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

            using (var application = AbpApplication.Create<IndependentEmptyModule>(services))
            {
                //Act

                application.Initialize(services.BuildServiceProvider());

                //Assert

                var module = application.ServiceProvider.GetRequiredService<IndependentEmptyModule>();
                module.ConfigureServicesIsCalled.ShouldBeTrue();
                module.OnApplicationInitializeIsCalled.ShouldBeTrue();
            }
        }

        [Fact]
        public void Should_Initialize_PlugIn()
        {
            //Arrange

            var services = new ServiceCollection();

            using (var application = AbpApplication.Create<IndependentEmptyModule>(services, options =>
            {
                options.PlugInSources.AddTypes(typeof(IndependentEmptyPlugInModule));
            }))
            {
                //Act

                application.Initialize(services.BuildServiceProvider());

                //Assert

                var plugInModule = application.ServiceProvider.GetRequiredService<IndependentEmptyPlugInModule>();
                plugInModule.ConfigureServicesIsCalled.ShouldBeTrue();
                plugInModule.OnApplicationInitializeIsCalled.ShouldBeTrue();
            }
        }
    }
}