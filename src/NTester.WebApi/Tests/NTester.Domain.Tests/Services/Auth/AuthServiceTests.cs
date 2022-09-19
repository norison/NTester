using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Constants;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.DateTime;
using NTester.Domain.Services.Token;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Services.Auth;

[TestFixture]
public class AuthServiceTests
{
    private INTesterDbContext _dbContext;
    private ITokenService _tokenService;
    private IDateTimeService _dateTimeService;
    private AuthService _authService;
    private RefreshTokenSettings _refreshTokenSettings;

    [SetUp]
    public void SetUp()
    {
        _dbContext = Substitute.For<INTesterDbContext>();
        _tokenService = Substitute.For<ITokenService>();
        _dateTimeService = Substitute.For<IDateTimeService>();
        _refreshTokenSettings = new RefreshTokenSettings { LifeMonths = 6 };

        var options = Substitute.For<IOptions<RefreshTokenSettings>>();
        options.Value.Returns(_refreshTokenSettings);

        _authService = new AuthService(_dbContext, _tokenService, _dateTimeService, options);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_ClientNotFound_ShouldThrowAnException(Guid userId, string clientName)
    {
        // Act/Assert
        var clientsDbSet = Array.Empty<ClientEntity>().AsQueryable().BuildMockDbSet();
        _dbContext.Clients.Returns(clientsDbSet);

        await _authService
            .Invoking(x => x.AuthenticateUserAsync(userId, clientName))
            .Should()
            .ThrowAsync<UnsupportedClientException>();
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_NoErrors_ShouldAuthenticate(
        Guid userId,
        string clientName,
        ClientEntity clientEntity,
        DateTime expirationDateTime,
        string accessToken,
        string refreshToken)
    {
        // Arrange
        RefreshTokenEntity capturedRefreshTokenEntity = null!;
        IList<Claim> capturedClaims = null!;

        var refreshTokenEntities = new List<RefreshTokenEntity> { new() { UserId = userId, ClientName = clientName } };
        var refreshTokensDbSet = refreshTokenEntities.AsQueryable().BuildMockDbSet();
        refreshTokensDbSet
            .WhenForAnyArgs(x => x.AddAsync(default!))
            .Do(x => capturedRefreshTokenEntity = x.Arg<RefreshTokenEntity>());

        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);

        clientEntity.Name = clientName;
        var clientsDbSet = new List<ClientEntity> { clientEntity }.AsQueryable().BuildMockDbSet();
        _dbContext.Clients.Returns(clientsDbSet);

        _tokenService
            .GenerateAccessToken(default!)
            .ReturnsForAnyArgs(accessToken)
            .AndDoes(x => capturedClaims = x.Arg<IList<Claim>>());
        _tokenService.GenerateRefreshToken().ReturnsForAnyArgs(refreshToken);

        _dateTimeService.UtcNow.Returns(expirationDateTime);

        //Act
        var result = await _authService.AuthenticateUserAsync(userId, clientName);

        // Assert
        refreshTokensDbSet.Received().Remove(refreshTokenEntities[0]);

        _tokenService.Received().GenerateAccessToken(capturedClaims);
        _tokenService.Received().GenerateRefreshToken();
        
        await refreshTokensDbSet.Received().AddAsync(capturedRefreshTokenEntity);
        await _dbContext.Received().SaveChangesAsync();

        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);

        capturedRefreshTokenEntity.Token.Should().Be(refreshToken);
        capturedRefreshTokenEntity.UserId.Should().Be(userId);
        capturedRefreshTokenEntity.ClientName.Should().Be(clientName);
        capturedRefreshTokenEntity.ExpirationDateTime
            .Should()
            .Be(expirationDateTime.AddMonths(_refreshTokenSettings.LifeMonths));
        capturedClaims
            .Should()
            .Contain(x => x.Type == ClaimConstants.UserIdClaimTypeName && x.Value == userId.ToString());
        capturedClaims
            .Should()
            .Contain(x => x.Type == ClaimConstants.ClientNameClaimTypeName && x.Value == clientName);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_RefreshTokenNotFound_ShouldThrowAnException(
        Guid userId,
        string clientName,
        string accessToken,
        string refreshToken)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
            new(ClaimConstants.ClientNameClaimTypeName, clientName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        _tokenService.GetPrincipalFromExpiredAccessToken(default!).ReturnsForAnyArgs(principal);
        var refreshTokensDbSet = new List<RefreshTokenEntity>().AsQueryable().BuildMockDbSet();
        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);

        // Act/Assert
        await _authService
            .Invoking(x => x.AuthenticateUserAsync(accessToken, refreshToken))
            .Should()
            .ThrowAsync<InvalidRefreshTokenException>();

        _tokenService.Received().GetPrincipalFromExpiredAccessToken(accessToken);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_RefreshTokenFoundWithAnotherTokenValue_ShouldThrowAnException(
        Guid userId,
        string clientName,
        string accessToken,
        string refreshToken,
        string anotherRefreshToken)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
            new(ClaimConstants.ClientNameClaimTypeName, clientName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var refreshTokenEntities = new List<RefreshTokenEntity>
        {
            new() { ClientName = clientName, UserId = userId, Token = anotherRefreshToken }
        };

        _tokenService.GetPrincipalFromExpiredAccessToken(default!).ReturnsForAnyArgs(principal);
        var refreshTokensDbSet = refreshTokenEntities.AsQueryable().BuildMockDbSet();
        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);

        // Act/Assert
        await _authService
            .Invoking(x => x.AuthenticateUserAsync(accessToken, refreshToken))
            .Should()
            .ThrowAsync<InvalidRefreshTokenException>();

        _tokenService.Received().GetPrincipalFromExpiredAccessToken(accessToken);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_NoError_ShouldAuthenticate(
        Guid userId,
        string clientName,
        string accessToken,
        string oldRefreshToken,
        string refreshToken,
        DateTime expirationDateTime)
    {
        // Arrange
        RefreshTokenEntity capturedRefreshTokenEntity = null!;
        IEnumerable<Claim> capturedClaims = null!;

        var claims = new List<Claim>
        {
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
            new(ClaimConstants.ClientNameClaimTypeName, clientName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var refreshTokenEntities = new List<RefreshTokenEntity>
        {
            new() { ClientName = clientName, UserId = userId, Token = oldRefreshToken }
        };

        _tokenService.GetPrincipalFromExpiredAccessToken(default!).ReturnsForAnyArgs(principal);
        _tokenService.GenerateRefreshToken().Returns(refreshToken);
        _tokenService
            .GenerateAccessToken(default!)
            .ReturnsForAnyArgs(accessToken)
            .AndDoes(x => capturedClaims = x.Arg<IEnumerable<Claim>>());

        var refreshTokensDbSet = refreshTokenEntities.AsQueryable().BuildMockDbSet();
        refreshTokensDbSet
            .WhenForAnyArgs(x => x.AddAsync(default!))
            .Do(x => capturedRefreshTokenEntity = x.Arg<RefreshTokenEntity>());

        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);
        _dateTimeService.UtcNow.Returns(expirationDateTime);

        // Act
        var result = await _authService.AuthenticateUserAsync(accessToken, oldRefreshToken);

        // Assert
        _tokenService.Received().GetPrincipalFromExpiredAccessToken(accessToken);
        _tokenService.Received().GenerateAccessToken(capturedClaims);
        _tokenService.Received().GenerateRefreshToken();

        await refreshTokensDbSet.Received().AddAsync(capturedRefreshTokenEntity);
        refreshTokensDbSet.Received().Remove(refreshTokenEntities[0]);

        await _dbContext.Received().SaveChangesAsync();

        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);

        capturedRefreshTokenEntity.Token.Should().Be(refreshToken);
        capturedRefreshTokenEntity.UserId.Should().Be(userId);
        capturedRefreshTokenEntity.ClientName.Should().Be(clientName);
        capturedRefreshTokenEntity.ExpirationDateTime
            .Should()
            .Be(expirationDateTime.AddMonths(_refreshTokenSettings.LifeMonths));
    }

    [Test, TestCaseSource(nameof(InvalidRefreshTokensTestCases))]
    public async Task RevokeRefreshTokenAsync_RefreshTokenNotFoundOrDoesNotMatch_ShouldThrowAnException(
        RefreshTokenEntity refreshTokenEntity,
        string refreshToken,
        string clientName,
        Guid userId)
    {
        // Arrange
        _dbContext.RefreshTokens.FindAsync(default).ReturnsForAnyArgs(refreshTokenEntity);

        // Act/Assert
        await _authService
            .Invoking(x => x.RevokeRefreshTokenAsync(refreshToken, userId, clientName))
            .Should()
            .ThrowAsync<InvalidRefreshTokenException>();
    }

    [Test, AutoDataExt]
    public async Task RevokeRefreshTokenAsync_RefreshTokenFound_ShouldRemoveRefreshToken(
        string refreshToken,
        string clientName,
        Guid userId)
    {
        // Arrange
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            ClientName = clientName,
            UserId = userId
        };
        _dbContext.RefreshTokens.FindAsync(default).ReturnsForAnyArgs(refreshTokenEntity);

        // Act
        await _authService.RevokeRefreshTokenAsync(refreshToken, userId, clientName);

        // Assert
        await _dbContext.RefreshTokens.Received().FindAsync(refreshToken);
        _dbContext.RefreshTokens.Received().Remove(refreshTokenEntity);
        await _dbContext.Received().SaveChangesAsync();
    }

    private static IEnumerable<TestCaseData> InvalidRefreshTokensTestCases => new List<TestCaseData>
    {
        new(null, string.Empty, string.Empty, Guid.NewGuid()),
        new(new RefreshTokenEntity
        {
            Token = "token",
            ExpirationDateTime = default,
            ClientName = "client",
            UserId = Guid.NewGuid()
        }, "token", "client", Guid.NewGuid()),
        new(new RefreshTokenEntity
        {
            Token = "token",
            ExpirationDateTime = default,
            ClientName = "client",
            UserId = Guid.Parse("29F79553-FC90-497C-BB2A-55179BF3BA6D")
        }, "token", string.Empty, Guid.Parse("29F79553-FC90-497C-BB2A-55179BF3BA6D"))
    };
}