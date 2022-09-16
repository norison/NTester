using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Constants;
using NTester.Domain.Exceptions;
using NTester.Domain.Exceptions.Codes;
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
    public async Task AuthenticateUserAsync_ClientNotFound_ShouldThrowAnException(UserEntity user, Guid clientId)
    {
        // Act/Assert
        await _authService
            .Invoking(x => x.AuthenticateUserAsync(user, clientId))
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(x => x.Code == (int)AuthCodes.UnsupportedClient);

        await _dbContext.Clients.Received().FindAsync(clientId);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_NoErrors_ShouldAuthenticate(
        UserEntity user,
        Guid clientId,
        ClientEntity clientEntity,
        DateTime expirationDateTime,
        string accessToken,
        string refreshToken)
    {
        // Arrange
        RefreshTokenEntity capturedRefreshTokenEntity = null!;
        IList<Claim> capturedClaims = null!;

        var refreshTokenEntities = new List<RefreshTokenEntity> { new() { UserId = user.Id, ClientId = clientId } };
        var refreshTokensDbSet = refreshTokenEntities.AsQueryable().BuildMockDbSet();
        refreshTokensDbSet
            .WhenForAnyArgs(x => x.AddAsync(default!))
            .Do(x => capturedRefreshTokenEntity = x.Arg<RefreshTokenEntity>());

        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);
        _dbContext.Clients.FindAsync(default).ReturnsForAnyArgs(clientEntity);

        _tokenService
            .GenerateAccessToken(default!)
            .ReturnsForAnyArgs(accessToken)
            .AndDoes(x => capturedClaims = x.Arg<IList<Claim>>());
        _tokenService.GenerateRefreshToken().ReturnsForAnyArgs(refreshToken);

        _dateTimeService.UtcNow.Returns(expirationDateTime);

        //Act
        var result = await _authService.AuthenticateUserAsync(user, clientId);

        // Assert
        await _dbContext.Clients.Received().FindAsync(clientId);

        await refreshTokensDbSet.Received().AddAsync(capturedRefreshTokenEntity);
        refreshTokensDbSet.Received().Remove(refreshTokenEntities[0]);

        _tokenService.Received().GenerateAccessToken(capturedClaims);
        _tokenService.Received().GenerateRefreshToken();
        
        await _dbContext.Received().SaveChangesAsync();

        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(refreshToken);

        capturedRefreshTokenEntity.Token.Should().Be(refreshToken);
        capturedRefreshTokenEntity.UserId.Should().Be(user.Id);
        capturedRefreshTokenEntity.ClientId.Should().Be(clientId);
        capturedRefreshTokenEntity.ExpirationDateTime
            .Should()
            .Be(expirationDateTime.AddMonths(_refreshTokenSettings.LifeMonths));
        capturedClaims
            .Should()
            .Contain(x => x.Type == ClaimConstants.UserIdClaimTypeName && x.Value == user.Id.ToString());
        capturedClaims
            .Should()
            .Contain(x => x.Type == ClaimConstants.ClientIdClaimTypeName && x.Value == clientId.ToString());
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_RefreshTokenNotFound_ShouldThrowAnException(
        Guid userId,
        Guid clientId,
        string accessToken,
        string refreshToken)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
            new(ClaimConstants.ClientIdClaimTypeName, clientId.ToString()),
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        _tokenService.GetPrincipalFromExpiredAccessToken(default!).ReturnsForAnyArgs(principal);
        var refreshTokensDbSet = new List<RefreshTokenEntity>().AsQueryable().BuildMockDbSet();
        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);

        // Act/Assert
        await _authService
            .Invoking(x => x.AuthenticateUserAsync(accessToken, refreshToken))
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(x => x.Code == (int)AuthCodes.InvalidRefreshToken);

        _tokenService.Received().GetPrincipalFromExpiredAccessToken(accessToken);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_RefreshTokenFoundWithAnotherTokenValue_ShouldThrowAnException(
        Guid userId,
        Guid clientId,
        string accessToken,
        string refreshToken,
        string anotherRefreshToken)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
            new(ClaimConstants.ClientIdClaimTypeName, clientId.ToString()),
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var refreshTokenEntities = new List<RefreshTokenEntity>
        {
            new() { ClientId = clientId, UserId = userId, Token = anotherRefreshToken }
        };

        _tokenService.GetPrincipalFromExpiredAccessToken(default!).ReturnsForAnyArgs(principal);
        var refreshTokensDbSet = refreshTokenEntities.AsQueryable().BuildMockDbSet();
        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);

        // Act/Assert
        await _authService
            .Invoking(x => x.AuthenticateUserAsync(accessToken, refreshToken))
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(x => x.Code == (int)AuthCodes.InvalidRefreshToken);

        _tokenService.Received().GetPrincipalFromExpiredAccessToken(accessToken);
    }

    [Test, AutoDataExt]
    public async Task AuthenticateUserAsync_NoError_ShouldAuthenticate(
        Guid userId,
        Guid clientId,
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
            new(ClaimConstants.ClientIdClaimTypeName, clientId.ToString()),
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var refreshTokenEntities = new List<RefreshTokenEntity>
        {
            new() { ClientId = clientId, UserId = userId, Token = oldRefreshToken }
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
        capturedRefreshTokenEntity.ClientId.Should().Be(clientId);
        capturedRefreshTokenEntity.ExpirationDateTime
            .Should()
            .Be(expirationDateTime.AddMonths(_refreshTokenSettings.LifeMonths));
    }

    [Test, TestCaseSource(nameof(InvalidRefreshTokensTestCases))]
    public async Task RevokeRefreshTokenAsync_RefreshTokenNotFoundOrDoesNotMatch_ShouldThrowAnException(
        RefreshTokenEntity refreshTokenEntity,
        string refreshToken,
        Guid clientId,
        Guid userId)
    {
        // Arrange
        _dbContext.RefreshTokens.FindAsync(default).ReturnsForAnyArgs(refreshTokenEntity);

        // Act/Assert
        await _authService
            .Invoking(x => x.RevokeRefreshTokenAsync(refreshToken, clientId, userId))
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(x => x.Code == (int)AuthCodes.InvalidRefreshToken);
    }

    [Test, AutoDataExt]
    public async Task RevokeRefreshTokenAsync_RefreshTokenFound_ShouldRemoveRefreshToken(
        string refreshToken,
        Guid clientId,
        Guid userId)
    {
        // Arrange
        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            ClientId = clientId,
            UserId = userId
        };
        _dbContext.RefreshTokens.FindAsync(default).ReturnsForAnyArgs(refreshTokenEntity);

        // Act
        await _authService.RevokeRefreshTokenAsync(refreshToken, clientId, userId);

        // Assert
        await _dbContext.RefreshTokens.Received().FindAsync(refreshToken);
        _dbContext.RefreshTokens.Received().Remove(refreshTokenEntity);
        await _dbContext.Received().SaveChangesAsync();
    }

    private static IEnumerable<TestCaseData> InvalidRefreshTokensTestCases => new List<TestCaseData>
    {
        new(null, string.Empty, Guid.NewGuid(), Guid.NewGuid()),
        new(new RefreshTokenEntity
        {
            Token = "token",
            ExpirationDateTime = default,
            ClientId = Guid.Parse("29F79553-FC90-497C-BB2A-55179BF3BA6D"),
            UserId = Guid.NewGuid(),
        }, "token", Guid.Parse("29F79553-FC90-497C-BB2A-55179BF3BA6D"), Guid.NewGuid()),
        new(new RefreshTokenEntity
        {
            Token = "token",
            ExpirationDateTime = default,
            ClientId = Guid.NewGuid(),
            UserId = Guid.Parse("29F79553-FC90-497C-BB2A-55179BF3BA6D"),
        }, "token", Guid.NewGuid(), Guid.Parse("29F79553-FC90-497C-BB2A-55179BF3BA6D"))
    };
}