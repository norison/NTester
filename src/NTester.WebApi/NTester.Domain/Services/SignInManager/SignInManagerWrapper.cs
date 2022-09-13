using Microsoft.AspNetCore.Identity;
using NTester.DataAccess.Entities;

namespace NTester.Domain.Services.SignInManager;

/// <inheritdoc cref="ISignInManager"/>
public class SignInManagerWrapper : ISignInManager
{
    private readonly SignInManager<UserEntity> _signInManager;

    /// <summary>
    /// Creates an instance of the sign in manager wrapper.
    /// </summary>
    /// <param name="signInManager">Sign in manager.</param>
    public SignInManagerWrapper(SignInManager<UserEntity> signInManager)
    {
        _signInManager = signInManager;
    }

    /// <inheritdoc cref="ISignInManager.CheckPasswordSignInAsync"/>
    public async Task<SignInResult> CheckPasswordSignInAsync(UserEntity user, string password, bool lockoutOnFailure)
    {
        return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
    }
}