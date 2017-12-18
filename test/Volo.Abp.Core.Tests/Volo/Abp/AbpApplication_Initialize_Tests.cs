using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Modularity.PlugIns;
using Xunit;

namespace Volo.Abp
{
    public class AbpApplication_Initialize_Tests
    {
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
                plugInModule.OnApplicationInitializeIsCalled.ShouldBeTrue();

                //Act
                application.Shutdown();

                //Assert
                plugInModule.OnApplicationShutdownIsCalled.ShouldBeTrue();
            }
        }
    }
}