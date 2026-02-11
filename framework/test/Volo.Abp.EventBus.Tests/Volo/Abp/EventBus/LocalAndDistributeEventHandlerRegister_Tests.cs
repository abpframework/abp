using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Xunit;

namespace Volo.Abp.EventBus;

public class LocalAndDistributeEventHandlerRegister_Tests : EventBusTestBase
{
    [Fact]
    public void Should_Register_Both_Local_And_Distribute()
    {
        var localOptions = GetRequiredService<IOptions<AbpLocalEventBusOptions>>();
        var distributedOptions = GetRequiredService<IOptions<AbpDistributedEventBusOptions>>();

        localOptions.Value.Handlers.ShouldContain(x => x == typeof(MyEventHandle));
        distributedOptions.Value.Handlers.ShouldContain(x => x == typeof(MyEventHandle));
    }

    class MyEventDate
    {

    }

    class MyEventHandle : ILocalEventHandler<MyEventDate>, IDistributedEventHandler<MyEventDate>, ITransientDependency
    {
        Task ILocalEventHandler<MyEventDate>.HandleEventAsync(MyEventDate eventData)
        {
            return Task.CompletedTask;
        }

        Task IDistributedEventHandler<MyEventDate>.HandleEventAsync(MyEventDate eventData)
        {
            return Task.CompletedTask;
        }
    }
}
