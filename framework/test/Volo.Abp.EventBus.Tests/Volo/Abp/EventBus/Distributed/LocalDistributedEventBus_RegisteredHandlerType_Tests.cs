using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Distributed;

public class LocalDistributedEventBus_RegisteredHandlerType_Tests : LocalDistributedEventBusTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        // Registered with Autofac directly, so the handler is not also subscribed to the application's bus by convention.
        var builder = services.GetObject<ContainerBuilder>();
        builder.RegisterType<CreationCounter>().SingleInstance();
        builder.RegisterType<CountedDistributedEventHandler>();
    }

    [Fact]
    public async Task A_Distributed_Ioc_Handler_Should_Be_Created_Once_Per_Publish()
    {
        var counter = GetRequiredService<CreationCounter>();
        var factory = new IocEventHandlerFactory(GetRequiredService<IServiceScopeFactory>(), typeof(CountedDistributedEventHandler));

        using (DistributedEventBus.Subscribe(typeof(CountedDistributedEventData), factory))
        {
            for (var i = 0; i < 3; i++)
            {
                await DistributedEventBus.PublishAsync(new CountedDistributedEventData(), onUnitOfWorkComplete: false);
            }
        }

        counter.Handled.ShouldBe(3);
        counter.Created.ShouldBe(3);
    }

    public class CountedDistributedEventData
    {
    }

    public class CreationCounter
    {
        private int _created;
        private int _handled;

        public int Created => _created;

        public int Handled => _handled;

        public void OnCreated() => Interlocked.Increment(ref _created);

        public void OnHandled() => Interlocked.Increment(ref _handled);
    }

    public class CountedDistributedEventHandler : IDistributedEventHandler<CountedDistributedEventData>
    {
        private readonly CreationCounter _counter;

        public CountedDistributedEventHandler(CreationCounter counter)
        {
            _counter = counter;
            _counter.OnCreated();
        }

        public Task HandleEventAsync(CountedDistributedEventData eventData)
        {
            _counter.OnHandled();
            return Task.CompletedTask;
        }
    }
}
