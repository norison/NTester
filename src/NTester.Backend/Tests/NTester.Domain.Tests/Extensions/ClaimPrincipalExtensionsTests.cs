using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using NTester.Domain.Constants;
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
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString())
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

    [Test, AutoData]
    public void GetClientName_Exists_ShouldReturnUserId(string clientName)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimConstants.ClientNameClaimTypeName, clientName)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        // Act
        var result = principal.GetClientName();

        // Assert
        result.Should().Be(clientName);
    }

    [Test]
    public void GetClientName_NotExists_ShouldReturnEmptyGuid()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        var result = principal.GetClientName();

        // Assert
        result.Should().BeNull();
    }
}