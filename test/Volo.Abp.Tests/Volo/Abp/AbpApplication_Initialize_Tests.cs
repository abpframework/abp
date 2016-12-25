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
            //Arrange
            var services = new ServiceCollection();
            var application = services.AddApplication<IndependentEmptyModule>();
            
            //Assert
            var module = services.GetSingletonInstance<IndependentEmptyModule>();
            module.PreConfigureServicesIsCalled.ShouldBeTrue();
            module.ConfigureServicesIsCalled.ShouldBeTrue();
            module.PostConfigureServicesIsCalled.ShouldBeTrue();

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                //Act
                application.Initialize(scope.ServiceProvider);
                
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
            //Arrange
            var services = new ServiceCollection();

            //Act
            var application = services.AddApplication<IndependentEmptyModule>(options =>
            {
                options.PlugInSources.AddTypes(typeof(IndependentEmptyPlugInModule));
            });

            //Assert
            var plugInModule = services.GetSingletonInstance<IndependentEmptyPlugInModule>();
            plugInModule.PreConfigureServicesIsCalled.ShouldBeTrue();
            plugInModule.ConfigureServicesIsCalled.ShouldBeTrue();
            plugInModule.PostConfigureServicesIsCalled.ShouldBeTrue();

            //Act
            application.Initialize(services.BuildServiceProvider());

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