using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.DependencyInjection
{
    public class AutoRegistrationHelper_Tests
    {
        [Fact]
        public void Should_Get_Conventional_Exposed_Types_By_Default()
        {
            //Act

            var exposedServices = AutoRegistrationHelper.GetExposedServices(new ServiceCollection(), typeof(DefaultDerivedService)).ToList();

            //Assert

            exposedServices.Count.ShouldBe(3);
            exposedServices.ShouldContain(typeof(DefaultDerivedService));
            exposedServices.ShouldContain(typeof(IService));
            exposedServices.ShouldContain(typeof(IDerivedService));
        }

        [Fact]
        public void Should_Get_Custom_Exposed_Types_If_Available()
        {
            //Act

            var exposedServices = AutoRegistrationHelper.GetExposedServices(new ServiceCollection(), typeof(ExplicitDerivedService)).ToList();

            //Assert

            exposedServices.Count.ShouldBe(1);
            exposedServices.ShouldContain(typeof(IDerivedService));
        }

        public class DefaultDerivedService : IDerivedService
        {
        }

        public interface IService
        {
        }

        public interface IDerivedService : IService
        {
        }

        [ExposeServices(typeof(IDerivedService))]
        public class ExplicitDerivedService : IDerivedService
        {
            
        }
    }
}
