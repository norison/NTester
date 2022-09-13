using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using NTester.DataAccess.Entities;

namespace NTester.Domain.Services.UserManager;

/// <inheritdoc cref="IUserManager"/>
[ExcludeFromCodeCoverage]
public class UserManagerWrapper : IUserManager
{
    private readonly UserManager<UserEntity> _userManager;

    /// <summary>
    /// Creates an instance of the user manager wrapper.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    public UserManagerWrapper(UserManager<UserEntity> userManager)
    {
        _userManager = userManager;
    }

    /// <inheritdoc cref="IUserManager.FindByNameAsync"/>
    public async Task<UserEntity> FindByNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

    /// <inheritdoc cref="IUserManager.CreateAsync"/>
    public async Task<IdentityResult> CreateAsync(UserEntity user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
}