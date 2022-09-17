using AutoMapper;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth.Login;
using NTester.DataContracts.Auth.Refresh;
using NTester.DataContracts.Auth.Register;
using NTester.Domain.Features.Auth.Commands.Login;
using NTester.Domain.Features.Auth.Commands.Refresh;
using NTester.Domain.Features.Auth.Commands.Register;

namespace NTester.Domain.Mappings.Auth;

/// <summary>
/// Mappings profile for the authentication feature.
/// </summary>
public class AuthMappingsProfile : Profile
{
    /// <summary>
    /// Creates and instance of the <see cref="AuthMappingsProfile"/>.
    /// </summary>
    public AuthMappingsProfile()
    {
        ConfigureLogin();
        ConfigureRegister();
        ConfigureRefresh();
    }

    private void ConfigureLogin()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }

    private void ConfigureRegister()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<RegisterCommand, UserEntity>().AfterMap((_, dest) => { dest.Id = Guid.NewGuid(); });
    }

    private void ConfigureRefresh()
    {
        CreateMap<RefreshRequest, RefreshCommand>();
    }
}