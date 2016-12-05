using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Tests.Modularity;
using Xunit;

namespace Volo.Abp.Tests
{
    public class AbpApplication_Tests
    {
        [Fact]
        public void Should_Initialize_SingleModule_Application()
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
    }
}
