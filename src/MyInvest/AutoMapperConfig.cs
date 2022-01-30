using AutoMapper;

namespace MyInvest;

public interface IMapperModule
{
    void Configure(IMapperConfigurationExpression config);
}

public class AutoMapperConfig
{
    private readonly List<IMapperModule> _modules;

    public AutoMapperConfig()
    {
        _modules = new List<IMapperModule>();
    }

    public AutoMapperConfig(IEnumerable<IMapperModule> modules)
    {
        _modules = new List<IMapperModule>(modules);
    }

    public AutoMapperConfig(IMapperModule module) : this()
    {
        _modules.Add(module);
    }

    public void RegisterModule(IMapperModule module)
    {
        _modules.Add(module);
    }

    public IMapper CreateMapper()
    {
        var config = new MapperConfiguration(ConfigureMapper);
        return config.CreateMapper();
    }

    private void ConfigureMapper(IMapperConfigurationExpression config)
    {
        _modules.ForEach(module => module.Configure(config));
    }
}