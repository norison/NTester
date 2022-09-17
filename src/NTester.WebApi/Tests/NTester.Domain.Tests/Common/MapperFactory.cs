using AutoMapper;
using NTester.Domain.Mappings.Account;
using NTester.Domain.Mappings.Auth;
using NTester.Domain.Mappings.Tests;

namespace NTester.Domain.Tests.Common;

public static class MapperFactory
{
    public static IMapper CreateMapper()
    {
        var mapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<AuthMappingsProfile>();
            config.AddProfile<AccountMappingsProfile>();
            config.AddProfile<TestsMappingsProfile>();
        });

        return new Mapper(mapperConfiguration);
    }
}