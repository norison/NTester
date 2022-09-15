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

        _dbContext.Clients.FindAsync(default).ReturnsForAnyArgs(clientEntity);
        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);

        _tokenService
            .GenerateAccessToken(default!)
            .ReturnsForAnyArgs(accessToken)
            .AndDoes(x => capturedClaims = x.Arg<IList<Claim>>());

        _tokenService.GenerateRefreshToken().ReturnsForAnyArgs(refreshToken);
        _dateTimeService.UtcNow.Returns(expirationDateTime);

        refreshTokensDbSet
            .WhenForAnyArgs(x => x.AddAsync(default!))
            .Do(x => capturedRefreshTokenEntity = x.Arg<RefreshTokenEntity>());

        //Act
        var result = await _authService.AuthenticateUserAsync(user, clientId);

        // Assert
        await _dbContext.Clients.Received().FindAsync(clientId);
        await refreshTokensDbSet.Received().AddAsync(capturedRefreshTokenEntity);
        refreshTokensDbSet.Received().Remove(refreshTokenEntities[0]);
        _tokenService.Received().GenerateAccessToken(capturedClaims);
        _tokenService.Received().GenerateRefreshToken();

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
        string refreshToken,
        string newRefreshToken,
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
            new() { ClientId = clientId, UserId = userId, Token = refreshToken }
        };

        _tokenService.GetPrincipalFromExpiredAccessToken(default!).ReturnsForAnyArgs(principal);
        var refreshTokensDbSet = refreshTokenEntities.AsQueryable().BuildMockDbSet();
        _dbContext.RefreshTokens.Returns(refreshTokensDbSet);
        
        _dateTimeService.UtcNow.Returns(expirationDateTime);

        _tokenService.GenerateRefreshToken().Returns(newRefreshToken);
        _tokenService
            .GenerateAccessToken(default!)
            .ReturnsForAnyArgs(accessToken)
            .AndDoes(x => capturedClaims = x.Arg<IEnumerable<Claim>>());

        refreshTokensDbSet
            .WhenForAnyArgs(x => x.AddAsync(default!))
            .Do(x => capturedRefreshTokenEntity = x.Arg<RefreshTokenEntity>());

        // Act
        var result = await _authService.AuthenticateUserAsync(accessToken, refreshToken);

        // Assert
        _tokenService.Received().GetPrincipalFromExpiredAccessToken(accessToken);
        await refreshTokensDbSet.Received().AddAsync(capturedRefreshTokenEntity);
        refreshTokensDbSet.Received().Remove(refreshTokenEntities[0]);
        _tokenService.Received().GenerateAccessToken(capturedClaims);
        _tokenService.Received().GenerateRefreshToken();

        result.AccessToken.Should().Be(accessToken);
        result.RefreshToken.Should().Be(newRefreshToken);
        capturedRefreshTokenEntity.Token.Should().Be(newRefreshToken);
        capturedRefreshTokenEntity.UserId.Should().Be(userId);
        capturedRefreshTokenEntity.ClientId.Should().Be(clientId);
        capturedRefreshTokenEntity.ExpirationDateTime
            .Should()
            .Be(expirationDateTime.AddMonths(_refreshTokenSettings.LifeMonths));
    }
}