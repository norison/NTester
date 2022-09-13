using Microsoft.AspNetCore.Identity;
using NTester.DataAccess.Entities;

namespace NTester.Domain.Services.SignInManager;

/// <summary>
/// Provides the APIs for user sign in.
/// </summary>
public interface ISignInManager
{
    /// <summary>
    /// Attempts a password sign in for a user.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="password">The password to attempt to sign in with.</param>
    /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails.</param>
    /// <returns>The task object representing the asynchronous operation containing the <see name="SignInResult"/>
    /// for the sign-in attempt.</returns>
    /// <returns></returns>
    Task<SignInResult> CheckPasswordSignInAsync(UserEntity user, string password, bool lockoutOnFailure);
}