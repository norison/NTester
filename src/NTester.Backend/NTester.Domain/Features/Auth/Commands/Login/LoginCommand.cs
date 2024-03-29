﻿using MediatR;
using NTester.DataContracts.Auth;

namespace NTester.Domain.Features.Auth.Commands.Login;

/// <summary>
/// Command for user login.
/// </summary>
public class LoginCommand : IRequest<AuthResponse>
{
    /// <summary>
    /// User name of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Name of the client.
    /// </summary>
    public string ClientName { get; set; } = string.Empty;
}