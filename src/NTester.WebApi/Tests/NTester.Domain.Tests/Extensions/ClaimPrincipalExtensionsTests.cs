using System.Security.Claims;
using FluentAssertions;
using NTester.Domain.Extensions;
using NUnit.Framework;

namespace NTester.Domain.Tests.Extensions;

[TestFixture]
public class ClaimPrincipalExtensionsTests
{
    [Test]
    public void GetUserId_Exists_ShouldReturnUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new(ClaimPrincipalExtensions.UserIdClaimType, userId.ToString())
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetUserId();

        // Assert
        result.Should().Be(userId);
    }
    
    [Test]
    public void GetUserId_NotExists_ShouldReturnEmptyGuid()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        var result = principal.GetUserId();

        // Assert
        result.Should().BeEmpty();
    }
    
    [Test]
    public void GetClientId_Exists_ShouldReturnUserId()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new(ClaimPrincipalExtensions.ClientIdClaimType, clientId.ToString())
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetClientId();

        // Assert
        result.Should().Be(clientId);
    }
    
    [Test]
    public void GetClientId_NotExists_ShouldReturnEmptyGuid()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        var result = principal.GetClientId();

        // Assert
        result.Should().BeEmpty();
    }
}