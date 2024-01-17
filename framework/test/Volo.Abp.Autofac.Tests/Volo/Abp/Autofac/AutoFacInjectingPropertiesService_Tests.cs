using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac.Interception;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.Autofac;

public class AutoFacInjectingPropertiesService_Tests : Autofac_Interception_Test
{
    [Fact]
    public void AutoFacInjectingPropertiesService_Should_Replaces_NullInjectingPropertiesService()
    {
        ServiceProvider.GetRequiredService<IInjectPropertiesService>().GetType().ShouldBe(typeof(AutoFacInjectPropertiesService));
    }

    [Fact]
    public void InjectProperties()
    {
        var injectPropertiesService = ServiceProvider.GetRequiredService<IInjectPropertiesService>();
        var serviceB = new TestServiceB();
        injectPropertiesService.InjectProperties(serviceB);

        serviceB.NullTestServiceA.ShouldNotBeNull();
        serviceB.NullTestServiceA.Name.ShouldBe("Default Name");
        serviceB.NotNullTestServiceA.ShouldNotBeNull();
        serviceB.NotNullTestServiceA.Name.ShouldBe("Default Name");
    }

    [Fact]
    public void InjectUnsetProperties()
    {
        var injectPropertiesService = ServiceProvider.GetRequiredService<IInjectPropertiesService>();
        var serviceB = new TestServiceB();
        injectPropertiesService.InjectUnsetProperties(serviceB);

        serviceB.NullTestServiceA.ShouldNotBeNull();
        serviceB.NullTestServiceA.Name.ShouldBe("Default Name");
        serviceB.NotNullTestServiceA.ShouldNotBeNull();
        serviceB.NotNullTestServiceA.Name.ShouldBe("My Name"); // This is not null property.
    }
}

interface ITestServiceA
{
    public string Name { get; set; }
}

class TestServiceA : ITestServiceA, ITransientDependency
{
    public string Name { get; set; } = "Default Name";
}

interface ITestServiceB
{

}

class TestServiceB : ITestServiceB, ITransientDependency
{
    public ITestServiceA NullTestServiceA { get; set; }

    public ITestServiceA NotNullTestServiceA { get; set; } = new TestServiceA()
    {
        Name = "My Name"
    };
}
