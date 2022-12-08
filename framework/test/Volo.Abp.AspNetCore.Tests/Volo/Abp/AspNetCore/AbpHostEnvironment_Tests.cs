using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
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

public class AbpHostEnvironment_Async_Initialize_Tests
{
    [Fact]
    public async Task Should_Set_Environment_From_AspNetCore()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Staging
        });
        builder.Host.UseAutofac();
        await builder.AddApplicationAsync<AbpAspNetCoreTestModule>();
        var app = builder.Build();
        await app.InitializeApplicationAsync();

        var abpHostEnvironment = app.Services.GetRequiredService<IAbpHostEnvironment>();
        abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Staging);

        builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Environments.Staging
        });
        builder.Host.UseAutofac();
        var abpApp = await AbpApplicationFactory.CreateAsync<AbpAspNetCoreTestModule>(builder.Services);
        app = builder.Build();
        await app.InitializeApplicationAsync();

        abpHostEnvironment = abpApp.Services.GetRequiredService<IAbpHostEnvironment>();
        abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Staging);
    }
}
