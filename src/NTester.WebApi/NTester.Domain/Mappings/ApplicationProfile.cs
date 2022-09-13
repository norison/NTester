using AutoMapper;
using NTester.DataContracts.Auth.Login;
using NTester.Domain.Features.Commands.Login;

namespace NTester.Domain.Mappings;

/// <summary>
/// Mapping profile for the entire application.
/// </summary>
public class ApplicationProfile : Profile
{
    /// <summary>
    /// Creates an instance of application mapping profile.
    /// </summary>
    public ApplicationProfile()
    {
        CreateMap<LoginRequest, LoginCommand>();
    }
}