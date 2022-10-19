using System.Net;
using FluentAssertions;
using NTester.Domain.Exceptions.Account;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Exceptions.Base;
using NTester.Domain.Exceptions.Common;
using NTester.Domain.Exceptions.Tests;
using NUnit.Framework;

namespace NTester.Domain.Tests.Exceptions;

[TestFixture]
public class ExceptionsTests
{
    [Test, TestCaseSource(nameof(ExceptionsTestCases))]
    public void ExceptionProperties_ShouldBeCorrectValues(
        RestException exception,
        HttpStatusCode statusCode,
        int code)
    {
        // Assert
        exception.StatusCode.Should().Be(statusCode);
        exception.Code.Should().Be(code);
    }

    private static IEnumerable<TestCaseData> ExceptionsTestCases => new List<TestCaseData>
    {
        // Common
        new(new NonGeneralException(string.Empty),
            HttpStatusCode.InternalServerError,
            (int)CommonCode.NonGeneralErrorOccured),
        new(new ModelValidationException(string.Empty),
            HttpStatusCode.BadRequest,
            (int)CommonCode.ModelValidationFailed),
        
        // Auth
        new(new InvalidUserNameOrPasswordException(),
            HttpStatusCode.BadRequest,
            (int)AuthCode.IncorrectUserNameOrPassword),
        new(new InvalidAccessTokenException(),
            HttpStatusCode.BadRequest,
            (int)AuthCode.InvalidAccessToken),
        new(new InvalidRefreshTokenException(),
            HttpStatusCode.BadRequest,
            (int)AuthCode.InvalidRefreshToken),
        new(new RefreshTokenWasNotProvidedException(),
            HttpStatusCode.BadRequest,
            (int)AuthCode.RefreshTokenWasNotProvided),
        new(new RefreshTokenExpiredException(),
            HttpStatusCode.BadRequest,
            (int)AuthCode.RefreshTokenExpired),
        new(new UnsupportedClientException(string.Empty),
            HttpStatusCode.BadRequest,
            (int)AuthCode.UnsupportedClient),
        
        // Account
        new(new UserNotFoundException(Guid.NewGuid()),
            HttpStatusCode.NotFound,
            (int)AccountCode.UserNotFound),
        
        // Tests
        new(new TestNotFoundException(Guid.NewGuid()),
            HttpStatusCode.NotFound,
            (int)TestsCode.TestNotFound)
    };
}