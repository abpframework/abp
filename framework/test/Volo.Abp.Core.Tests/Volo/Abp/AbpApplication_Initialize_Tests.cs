using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.DependencyInjection;
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

    [Fact]
    public void Should_Set_And_Get_ApplicationName_And_InstanceId()
    {
        var applicationName = "MyApplication";

        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>(options =>
               {
                   options.ApplicationName = applicationName;
               }))
        {
            application.ApplicationName.ShouldBe(applicationName);
            application.Services.GetApplicationName().ShouldBe(applicationName);

            application.Initialize();

            var appInfo = application.ServiceProvider.GetRequiredService<IApplicationInfoAccessor>();
            appInfo.ApplicationName.ShouldBe(applicationName);
            appInfo.InstanceId.ShouldNotBeNullOrEmpty();
        }

        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>(options =>
               {
                   options.Services.ReplaceConfiguration(new ConfigurationBuilder()
                       .AddInMemoryCollection(new Dictionary<string, string> {{"ApplicationName", applicationName}})
                       .Build());
               }))
        {

            application.ApplicationName.ShouldBe(applicationName);
            application.Services.GetApplicationName().ShouldBe(applicationName);

            application.Initialize();

            application.ServiceProvider
                .GetRequiredService<IApplicationInfoAccessor>()
                .ApplicationName
                .ShouldBe(applicationName);
        }

        applicationName = Assembly.GetEntryAssembly()?.GetName().Name;
        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>())
        {
            application.ApplicationName.ShouldBe(applicationName);
            application.Services.GetApplicationName().ShouldBe(applicationName);

            application.Initialize();

            application.ServiceProvider
                .GetRequiredService<IApplicationInfoAccessor>()
                .ApplicationName
                .ShouldBe(applicationName);
        }
    }

    [Fact]
    public void Should_Set_And_Get_Environment()
    {
        // Default environment is Production
        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>())
        {
            var abpHostEnvironment = application.Services.GetSingletonInstance<IAbpHostEnvironment>();
            abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Production);

            application.Initialize();

            abpHostEnvironment = application.ServiceProvider.GetRequiredService<IAbpHostEnvironment>();
            abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Production);
        }

        // Set environment
        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>(options =>
               {
                   options.Environment = Environments.Staging;
               }))
        {
            var abpHostEnvironment = application.Services.GetSingletonInstance<IAbpHostEnvironment>();
            abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Staging);

            application.Initialize();

            abpHostEnvironment = application.ServiceProvider.GetRequiredService<IAbpHostEnvironment>();
            abpHostEnvironment.EnvironmentName.ShouldBe(Environments.Staging);
        }
    }

    [Fact]
    public async Task Should_Resolve_Root_Service_Provider()
    {
        using (var application = await AbpApplicationFactory.CreateAsync<IndependentEmptyModule>())
        {
            await application.InitializeAsync();

            application
                .ServiceProvider
                .GetRequiredService<IRootServiceProvider>()
                .ShouldNotBeNull();
        }
    }
}
