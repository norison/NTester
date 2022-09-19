using AutoMapper;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Account.GetUser;

namespace NTester.Domain.Mappings;

/// <summary>
/// Mappings profile for the account feature.
/// </summary>
public class AccountMappingsProfile : Profile
{
    /// <summary>
    /// Creates and instance of the <see cref="AccountMappingsProfile"/>.
    /// </summary>
    public AccountMappingsProfile()
    {
        CreateMap<UserEntity, GetUserResponse>();
    }
}