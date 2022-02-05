using NUnit.Framework;

namespace MyInvest.UnitTests;

public class AutoMapperConfigFactoryTests
{
    [Test]
    public void CreatesValidConfig()
    {
        var config = AutoMapperConfigFactory.Create();
        config.AssertConfigurationIsValid();
    }
}
