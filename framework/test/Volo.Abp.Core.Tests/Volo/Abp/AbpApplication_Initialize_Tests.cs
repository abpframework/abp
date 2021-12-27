using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Modularity.PlugIns;
using Xunit;

namespace Volo.Abp;

public class AbpApplication_Initialize_Tests
{
    [Fact]
    public async Task Should_Initialize_Single_Module_Async()
    {
        using (var application = await AbpApplicationFactory.CreateAsync<IndependentEmptyModule>())
        {
            //Assert
            var module = application.Services.GetSingletonInstance<IndependentEmptyModule>();

            module.PreConfigureServicesAsyncIsCalled.ShouldBeTrue();
            module.PreConfigureServicesIsCalled.ShouldBeTrue();

            module.ConfigureServicesAsyncIsCalled.ShouldBeTrue();
            module.ConfigureServicesIsCalled.ShouldBeTrue();

            module.PostConfigureServicesAsyncIsCalled.ShouldBeTrue();
            module.PostConfigureServicesIsCalled.ShouldBeTrue();

            //Act
            await application.InitializeAsync();

            //Assert
            application.ServiceProvider.GetRequiredService<IndependentEmptyModule>().ShouldBeSameAs(module);
            module.OnApplicationInitializeAsyncIsCalled.ShouldBeTrue();
            module.OnApplicationInitializeIsCalled.ShouldBeTrue();
            //Act
            await application.ShutdownAsync();

            //Assert
            module.OnApplicationShutdownAsyncIsCalled.ShouldBeTrue();
            module.OnApplicationShutdownIsCalled.ShouldBeTrue();
        }
    }

    [Fact]
    public void Should_Initialize_Single_Module()
    {
        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>())
        {
            //Assert
            var module = application.Services.GetSingletonInstance<IndependentEmptyModule>();
            module.PreConfigureServicesIsCalled.ShouldBeTrue();
            module.ConfigureServicesIsCalled.ShouldBeTrue();
            module.PostConfigureServicesIsCalled.ShouldBeTrue();

            //Act
            application.Initialize();

            //Assert
            application.ServiceProvider.GetRequiredService<IndependentEmptyModule>().ShouldBeSameAs(module);
            module.OnApplicationInitializeIsCalled.ShouldBeTrue();

            //Act
            application.Shutdown();

            //Assert
            module.OnApplicationShutdownIsCalled.ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Should_Initialize_PlugIn_Async()
    {
        using (var application = await AbpApplicationFactory.CreateAsync<IndependentEmptyModule>(options =>
               {
                   options.PlugInSources.AddTypes(typeof(IndependentEmptyPlugInModule));
               }))
        {
            //Assert
            var plugInModule = application.Services.GetSingletonInstance<IndependentEmptyPlugInModule>();

            plugInModule.PreConfigureServicesAsyncIsCalled.ShouldBeTrue();
            plugInModule.PreConfigureServicesIsCalled.ShouldBeTrue();

            plugInModule.ConfigureServicesAsyncIsCalled.ShouldBeTrue();
            plugInModule.ConfigureServicesIsCalled.ShouldBeTrue();

            plugInModule.PostConfigureServicesAsyncIsCalled.ShouldBeTrue();
            plugInModule.PostConfigureServicesIsCalled.ShouldBeTrue();

            //Act
            await application.InitializeAsync();

            //Assert
            application.ServiceProvider.GetRequiredService<IndependentEmptyPlugInModule>().ShouldBeSameAs(plugInModule);

            plugInModule.OnPreApplicationInitializationAsyncIsCalled.ShouldBeTrue();
            plugInModule.OnPreApplicationInitializationIsCalled.ShouldBeTrue();

            plugInModule.OnApplicationInitializeAsyncIsCalled.ShouldBeTrue();
            plugInModule.OnApplicationInitializeIsCalled.ShouldBeTrue();

            plugInModule.OnPostApplicationInitializationAsyncIsCalled.ShouldBeTrue();
            plugInModule.OnPostApplicationInitializationIsCalled.ShouldBeTrue();

            //Act
            await application.ShutdownAsync();

            //Assert
            plugInModule.OnApplicationShutdownAsyncIsCalled.ShouldBeTrue();
            plugInModule.OnApplicationShutdownIsCalled.ShouldBeTrue();
        }
    }

    [Fact]
    public void Should_Initialize_PlugIn()
    {
        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>(options =>
        {
            options.PlugInSources.AddTypes(typeof(IndependentEmptyPlugInModule));
        }))
        {
            //Assert
            var plugInModule = application.Services.GetSingletonInstance<IndependentEmptyPlugInModule>();
            plugInModule.PreConfigureServicesIsCalled.ShouldBeTrue();
            plugInModule.ConfigureServicesIsCalled.ShouldBeTrue();
            plugInModule.PostConfigureServicesIsCalled.ShouldBeTrue();

            //Act
            application.Initialize();

            //Assert
            application.ServiceProvider.GetRequiredService<IndependentEmptyPlugInModule>().ShouldBeSameAs(plugInModule);
            plugInModule.OnPreApplicationInitializationIsCalled.ShouldBeTrue();
            plugInModule.OnApplicationInitializeIsCalled.ShouldBeTrue();
            plugInModule.OnPostApplicationInitializationIsCalled.ShouldBeTrue();

            //Act
            application.Shutdown();

            //Assert
            plugInModule.OnApplicationShutdownIsCalled.ShouldBeTrue();
        }
    }
}
