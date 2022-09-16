﻿using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when refresh token is invalid.
/// </summary>
public class InvalidRefreshTokenException : RestExceptionBase
{
    private const string ErrorMessage = "Refresh token not found or no access token pair.";

    /// <summary>
    /// Creates an instance of InvalidRefreshToken exception.
    /// </summary>
    public InvalidRefreshTokenException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.InvalidRefreshToken;
}