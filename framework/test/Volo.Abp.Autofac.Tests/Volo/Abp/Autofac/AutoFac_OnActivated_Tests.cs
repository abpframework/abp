using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac.Interception;
using Xunit;

namespace Volo.Abp.Autofac;

public class AutoFac_OnActivated_Tests : Autofac_Interception_Test
{
    protected override Task AfterAddApplicationAsync(IServiceCollection services)
    {
        var serviceDescriptor = ServiceDescriptor.Transient<MyServer, MyServer>();
        services.Add(serviceDescriptor);
        services.OnActivated(serviceDescriptor, x =>
        {
            x.Instance.As<MyServer>().Name += "1";
        });
        services.OnActivated(serviceDescriptor, x =>
        {
            x.Instance.As<MyServer>().Name += "2";
        });

        return base.AfterAddApplicationAsync(services);
    }

    [Fact]
    public void Should_Call_OnActivated()
    {
        var server = ServiceProvider.GetRequiredService<MyServer>();
        server.Name.ShouldBe("MyServer12");
    }
}

class MyServer
{
    public string Name { get; set; } = "MyServer";
}
