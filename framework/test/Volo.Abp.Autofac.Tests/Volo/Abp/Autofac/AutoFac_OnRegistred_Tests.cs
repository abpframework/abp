using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac.Interception;
using Xunit;

namespace Volo.Abp.Autofac;

public class AutoFac_OnRegistred_Tests : Autofac_Interception_Test
{
    protected override Task AfterAddApplicationAsync(IServiceCollection services)
    {
        services.Add(ServiceDescriptor.KeyedTransient<MyServer, MyServer>("key"));
        services.OnRegistered(onServiceRegistredContext =>
        {
            if (onServiceRegistredContext.ImplementationType == typeof(MyServer))
            {
                onServiceRegistredContext.ServiceKey.ShouldBe("key");
            }
        });

        return base.AfterAddApplicationAsync(services);
    }

    class MyServer
    {
        public string Name { get; set; } = "MyServer";
    }
}
