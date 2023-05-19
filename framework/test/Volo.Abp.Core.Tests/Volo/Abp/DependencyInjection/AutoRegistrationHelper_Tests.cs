using Shouldly;
using Xunit;

namespace Volo.Abp.DependencyInjection;

public class AutoRegistrationHelper_Tests
{
    [Fact]
    public void Should_Get_Conventional_Exposed_Types_By_Default()
    {
        //Act

        var exposedServices = ExposedServiceExplorer.GetExposedServices(typeof(DefaultDerivedService));

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

        var exposedServices = ExposedServiceExplorer.GetExposedServices(typeof(ExplicitDerivedService));

        //Assert

        exposedServices.Count.ShouldBe(1);
        exposedServices.ShouldContain(typeof(IDerivedService));
    }

    [Fact]
    public void Should_Get_Conventional_Exposed_Generic_Types_By_Default()
    {
        //Act
        var exposedServices = ExposedServiceExplorer.GetExposedServices(typeof(DefaultGenericService));

        //Assert
        exposedServices.Count.ShouldBe(4);
        exposedServices.ShouldContain(typeof(IService));
        exposedServices.ShouldContain(typeof(IGenericService<string>));
        exposedServices.ShouldContain(typeof(IGenericService<int>));
        exposedServices.ShouldContain(typeof(DefaultGenericService));
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

    public interface IGenericService<T>
    {

    }

    public class DefaultGenericService : IService, IGenericService<string>, IGenericService<int>
    {

    }
}
