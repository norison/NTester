using System.Net;
using FluentAssertions;
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
            (int)CommonCodes.NonGeneralErrorOccured),
        new(new ModelValidationException(string.Empty),
            HttpStatusCode.BadRequest,
            (int)CommonCodes.ModelValidationFailed),
        new(new UserAlreadyExistsException(string.Empty),
            HttpStatusCode.BadRequest,
            (int)AuthCodes.UserAlreadyExists),
        new(new IncorrectUserNameOrPasswordException(),
            HttpStatusCode.BadRequest,
            (int)AuthCodes.IncorrectUserNameOrPassword),
        new(new InvalidAccessTokenException(),
            HttpStatusCode.BadRequest,
            (int)AuthCodes.InvalidAccessToken),
        new(new InvalidRefreshTokenException(),
            HttpStatusCode.BadRequest,
            (int)AuthCodes.InvalidRefreshToken),
        new(new RefreshTokenWasNotProvidedException(),
            HttpStatusCode.BadRequest,
            (int)AuthCodes.RefreshTokenWasNotProvided),
        new(new UnsupportedClientException(Guid.NewGuid()),
            HttpStatusCode.BadRequest,
            (int)AuthCodes.UnsupportedClient)
    };
}