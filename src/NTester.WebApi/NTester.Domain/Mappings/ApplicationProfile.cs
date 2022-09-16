using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Account.GetUser;
using NTester.DataContracts.Auth.Login;
using NTester.DataContracts.Auth.Refresh;
using NTester.DataContracts.Auth.Register;
using NTester.Domain.Features.Auth.Commands.Login;
using NTester.Domain.Features.Auth.Commands.Refresh;
using NTester.Domain.Features.Auth.Commands.Register;

namespace NTester.Domain.Mappings;

/// <summary>
/// Mapping profile for the entire application.
/// </summary>
[ExcludeFromCodeCoverage]
public class ApplicationProfile : Profile
{
    /// <summary>
    /// Creates an instance of application mapping profile.
    /// </summary>
    public ApplicationProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<RefreshRequest, RefreshCommand>();
        CreateMap<UserEntity, GetUserResponse>();
    }
}