using AutoMapper;
using MyInvest.Persistence;
using MyInvest.REST;

namespace MyInvest;

public static class AutoMapperConfigFactory
{
    public static MapperConfiguration Create() => new(config =>
    {
        config.AddProfile<RestMapperProfile>();
        config.AddProfile<PersistenceMapperProfile>();
    });
}
