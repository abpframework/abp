using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore;

public class AbpHostEnvironment_Tests : AbpAspNetCoreTestBase<Startup>
{
    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = base.CreateHostBuilder();
        builder.ConfigureHostConfiguration(x => x.Sources.Insert(0,
            new MemoryConfigurationSource()
            {
                InitialData = new List<KeyValuePair<string, string>>
                {
                    new(HostDefaults.EnvironmentKey, Environments.Staging),
                }
            }));
        return builder;
    }

    [Fact]
    public void Should_Set_Environment_From_IWebHostEnvironment()
    {
        var abpHostEnvironment = GetRequiredService<IAbpHostEnvironment>();
        abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Staging);
    }
}
