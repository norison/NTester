using Microsoft.AspNetCore.Identity;
using NTester.DataAccess.Entities;

namespace NTester.Domain.Services.UserManager;

/// <summary>
/// Provides the APIs for managing user in a persistence store.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Returns an IQueryable of users if the store is an IQueryableUserStore
    /// </summary>
    IQueryable<UserEntity> Users { get; }
    
    /// <summary>
    /// Finds and returns a user, if any, who has the specified user name.
    /// </summary>
    /// <param name="userName">The user name to search for.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userName"/> if it exists.
    /// </returns>
    Task<UserEntity> FindByNameAsync(string userName);

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the backing store with given password,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="password">The password for the user to hash and store.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
    /// of the operation.
    /// </returns>
    Task<IdentityResult> CreateAsync(UserEntity user, string password);
}