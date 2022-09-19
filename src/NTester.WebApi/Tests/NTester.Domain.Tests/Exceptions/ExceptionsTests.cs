using System.Net;
using FluentAssertions;
using NTester.Domain.Exceptions.Account;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Exceptions.Base;
using NTester.Domain.Exceptions.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Exceptions;

[TestFixture]
public class ExceptionsTests
{
    [Test, TestCaseSource(nameof(ExceptionsTestCases))]
    public void ExceptionProperties_ShouldBeCorrectValues(
        RestExceptionBase exception,
        HttpStatusCode statusCode,
        int code)
    {
        // Assert
        exception.StatusCode.Should().Be(statusCode);
        exception.Code.Should().Be(code);
    }

    private static IEnumerable<TestCaseData> ExceptionsTestCases => new List<TestCaseData>
    {
        new(new NonGeneralException(string.Empty),
            HttpStatusCode.InternalServerError,
            (int)CommonCode.NonGeneralErrorOccured),
        new(new ModelValidationException(string.Empty),
            HttpStatusCode.BadRequest,
            (int)CommonCode.ModelValidationFailed),
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
        new(new UnsupportedClientException(string.Empty),
            HttpStatusCode.BadRequest,
            (int)AuthCode.UnsupportedClient),
        new(new UserNotFoundException(Guid.NewGuid()),
            HttpStatusCode.NotFound,
            (int)AccountCode.UserNotFound)
    };
}