using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_Order_Test : EventBusTestBase
{
    public static string HandlerExecuteOrder { get; set; }

    [Fact]
    public async Task Handler_Should_Execute_By_Order()
    {
        HandlerExecuteOrder = "";
        await LocalEventBus.PublishAsync(new MyOrderEventHandlerEventData());
        HandlerExecuteOrder.ShouldBe("321");
    }

    [Fact]
    public async Task An_Ioc_Handler_Should_Be_Created_Once_Per_Publish()
    {
        CountedOrderEventHandler.Reset();

        for (var i = 0; i < 3; i++)
        {
            await LocalEventBus.PublishAsync(new CountedOrderEventData());
        }

        // Its registration tells its type, so it is not created to read its order.
        CountedOrderEventHandler.Created.ShouldBe(3);
        CountedOrderEventHandler.Handled.ShouldBe(3);
        CountedOrderEventHandler.Disposed.ShouldBe(3);
    }

    [Fact]
    public void Listing_The_Factories_Should_Return_The_Subscribed_Factories_Without_Creating_A_Handler()
    {
        CountedOrderEventHandler.Reset();

        var factories = ((LocalEventBus)LocalEventBus).GetEventHandlerFactories(typeof(CountedOrderEventData));

        factories.SelectMany(x => x.EventHandlerFactories).ShouldContain(x => x is IocEventHandlerFactory);
        CountedOrderEventHandler.Created.ShouldBe(0);
        CountedOrderEventHandler.Disposed.ShouldBe(0);
        CountedOrderEventHandler.Handled.ShouldBe(0);
    }

    [Fact]
    public async Task Ordering_Should_Not_Create_A_Handler_Whose_Type_Is_Known()
    {
        CountedTransientEventHandler.Reset();
        using (LocalEventBus.Subscribe<CountedTransientEventData, CountedTransientEventHandler>())
        {
            for (var i = 0; i < 3; i++)
            {
                await LocalEventBus.PublishAsync(new CountedTransientEventData());
            }
        }

        CountedTransientEventHandler.Created.ShouldBe(3);
        CountedTransientEventHandler.Disposed.ShouldBe(3);
    }

    [Fact]
    public async Task Order_Should_Come_From_The_Handler_The_Factory_Creates()
    {
        HandlerExecuteOrder = "";
        using (LocalEventBus.Subscribe(typeof(ReplacedOrderEventData), new SwitchingHandlerFactory(new ReplacementOrderEventHandler())))
        {
            await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
        }

        HandlerExecuteOrder.ShouldBe("R0");
    }

    [Fact]
    public async Task Order_Should_Follow_The_Type_The_Factory_Creates_Now()
    {
        var factory = new SwitchingHandlerFactory(new ReplacementOrderEventHandler());
        using (LocalEventBus.Subscribe(typeof(ReplacedOrderEventData), factory))
        {
            HandlerExecuteOrder = "";
            await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
            HandlerExecuteOrder.ShouldBe("R0");

            factory.Handler = new OriginalOrderEventHandler();
            HandlerExecuteOrder = "";
            await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
            HandlerExecuteOrder.ShouldBe("0O");
        }
    }

    [Fact]
    public async Task A_Handler_That_Unsubscribes_When_Disposed_Should_Not_Break_The_Publish()
    {
        HandlerExecuteOrder = "";
        var factory = new SelfUnsubscribingHandlerFactory(LocalEventBus);
        LocalEventBus.Subscribe(typeof(ReplacedOrderEventData), factory);

        await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
        HandlerExecuteOrder.ShouldBe("R0");

        HandlerExecuteOrder = "";
        await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
        HandlerExecuteOrder.ShouldBe("0");
    }

    [Fact]
    public async Task A_Handler_Whose_Type_Is_Known_Should_Still_Be_Ordered()
    {
        HandlerExecuteOrder = "";
        using (LocalEventBus.Subscribe<ReplacedOrderEventData, ReplacementOrderEventHandler>())
        using (LocalEventBus.Subscribe(typeof(ReplacedOrderEventData), new TransientEventHandlerFactory(typeof(OriginalOrderEventHandler))))
        {
            await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
        }

        HandlerExecuteOrder.ShouldBe("R0O");
    }

    [Fact]
    public async Task A_Derived_Transient_Factory_Should_Be_Ordered_By_The_Handler_It_Creates()
    {
        HandlerExecuteOrder = "";
        using (LocalEventBus.Subscribe(typeof(ReplacedOrderEventData), new ReplacingTransientHandlerFactory()))
        {
            await LocalEventBus.PublishAsync(new ReplacedOrderEventData());
        }

        HandlerExecuteOrder.ShouldBe("R0");
    }

    [Fact]
    public async Task Typed_And_Dynamic_Handlers_Should_Be_Ordered_Together()
    {
        HandlerExecuteOrder = "";
        var eventName = EventNameAttribute.GetNameOrDefault<NamedOrderEventData>();
        using (LocalEventBus.Subscribe<NamedOrderEventData, LateTypedOrderEventHandler>())
        using (LocalEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new EarlyDynamicOrderEventHandler())))
        {
            await LocalEventBus.PublishAsync(new NamedOrderEventData());
        }

        HandlerExecuteOrder.ShouldBe("DT");
    }

    [Fact]
    public async Task A_Dynamic_Handler_That_Unsubscribes_When_Disposed_Should_Not_Break_The_Publish()
    {
        var eventName = "UnmappedOrderEvent-" + Guid.NewGuid().ToString("N");
        LocalEventBus.Subscribe(eventName, new SelfUnsubscribingDynamicHandlerFactory(LocalEventBus, eventName));
        using (LocalEventBus.Subscribe(eventName, new SingleInstanceHandlerFactory(new LateDynamicOrderEventHandler())))
        {
            HandlerExecuteOrder = "";
            await LocalEventBus.PublishAsync(eventName, new { Value = 1 });
            HandlerExecuteOrder.ShouldBe("DL");

            HandlerExecuteOrder = "";
            await LocalEventBus.PublishAsync(eventName, new { Value = 2 });
            HandlerExecuteOrder.ShouldBe("L");
        }
    }

    [Fact]
    public async Task An_Ioc_Handler_Replaced_In_A_Container_That_Can_Not_Tell_Its_Type_Should_Be_Ordered_By_The_Replacement()
    {
        var services = new ServiceCollection();
        services.AddTransient<OriginalIocOrderEventHandler, ReplacementIocOrderEventHandler>();
        await using var provider = services.BuildServiceProvider();
        ReplacementIocOrderEventHandler.Reset();

        HandlerExecuteOrder = "";
        using (LocalEventBus.Subscribe(typeof(IocReplacedOrderEventData), new IocEventHandlerFactory(provider.GetRequiredService<IServiceScopeFactory>(), typeof(OriginalIocOrderEventHandler))))
        using (LocalEventBus.Subscribe<IocReplacedOrderEventData, ZeroOrderIocEventHandler>())
        {
            await LocalEventBus.PublishAsync(new IocReplacedOrderEventData());
        }

        HandlerExecuteOrder.ShouldBe("R0");
        // Microsoft's container does not tell the type, so the handler is also created to read its order.
        ReplacementIocOrderEventHandler.Created.ShouldBe(2);
        ReplacementIocOrderEventHandler.Disposed.ShouldBe(2);
    }

    [Fact]
    public async Task Publishing_While_Others_Subscribe_And_Unsubscribe_Should_Not_Fail()
    {
        var handled = 0;
        using var stable = LocalEventBus.Subscribe<ConcurrentOrderEventData>(_ =>
        {
            Interlocked.Increment(ref handled);
            return Task.CompletedTask;
        });

        var publishers = Enumerable.Range(0, 4).Select(_ => Task.Run(async () =>
        {
            for (var i = 0; i < 200; i++)
            {
                await LocalEventBus.PublishAsync(new ConcurrentOrderEventData());
            }
        }));
        var subscribers = Enumerable.Range(0, 4).Select(_ => Task.Run(() =>
        {
            for (var i = 0; i < 200; i++)
            {
                LocalEventBus.Subscribe<ConcurrentOrderEventData, ConcurrentOrderEventHandler>().Dispose();
            }
        }));

        await Task.WhenAll(publishers.Concat(subscribers));

        handled.ShouldBe(800);
    }

    public class MyOrderEventHandlerEventData
    {

    }

    public class CountedOrderEventData
    {

    }

    public class CountedOrderEventHandler : ILocalEventHandler<CountedOrderEventData>, ITransientDependency, IDisposable
    {
        private static int _created;
        private static int _disposed;

        public static int Created => _created;

        public static int Disposed => _disposed;

        public static int Handled => _handled;

        private static int _handled;

        public CountedOrderEventHandler()
        {
            Interlocked.Increment(ref _created);
        }

        public static void Reset()
        {
            _created = 0;
            _disposed = 0;
            _handled = 0;
        }

        public Task HandleEventAsync(CountedOrderEventData eventData)
        {
            Interlocked.Increment(ref _handled);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Interlocked.Increment(ref _disposed);
        }
    }

    public class CountedTransientEventData
    {

    }

    public class CountedTransientEventHandler : ILocalEventHandler<CountedTransientEventData>, IDisposable
    {
        private static int _created;
        private static int _disposed;

        public static int Created => _created;

        public static int Disposed => _disposed;

        public CountedTransientEventHandler()
        {
            Interlocked.Increment(ref _created);
        }

        public static void Reset()
        {
            _created = 0;
            _disposed = 0;
        }

        public Task HandleEventAsync(CountedTransientEventData eventData)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Interlocked.Increment(ref _disposed);
        }
    }

    public class ReplacedOrderEventData
    {

    }

    public class ZeroOrderEventHandler : ILocalEventHandler<ReplacedOrderEventData>, ITransientDependency
    {
        public Task HandleEventAsync(ReplacedOrderEventData eventData)
        {
            HandlerExecuteOrder += "0";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(10)]
    public class OriginalOrderEventHandler : ILocalEventHandler<ReplacedOrderEventData>
    {
        public virtual Task HandleEventAsync(ReplacedOrderEventData eventData)
        {
            HandlerExecuteOrder += "O";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-10)]
    public class ReplacementOrderEventHandler : OriginalOrderEventHandler
    {
        public override Task HandleEventAsync(ReplacedOrderEventData eventData)
        {
            HandlerExecuteOrder += "R";
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Registered for <see cref="OriginalOrderEventHandler"/>, but creates its replacement.
    /// </summary>
    public class ReplacingTransientHandlerFactory : TransientEventHandlerFactory<OriginalOrderEventHandler>
    {
        protected override IEventHandler CreateHandler()
        {
            return new ReplacementOrderEventHandler();
        }
    }

    /// <summary>
    /// Unsubscribes itself when its first handler is disposed, which ordering does before the handler runs.
    /// </summary>
    public class SelfUnsubscribingHandlerFactory : IEventHandlerFactory
    {
        private readonly ILocalEventBus _localEventBus;

        public SelfUnsubscribingHandlerFactory(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            return new EventHandlerDisposeWrapper(
                new ReplacementOrderEventHandler(),
                () => _localEventBus.Unsubscribe(typeof(ReplacedOrderEventData), this));
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories.Contains(this);
        }
    }

    /// <summary>
    /// A factory whose handler type is not the one it was registered for and may change, as a replaced or
    /// conditionally registered service can: the order is read from the handler it creates now.
    /// </summary>
    public class SwitchingHandlerFactory : IEventHandlerFactory
    {
        public IEventHandler Handler { get; set; }

        public SwitchingHandlerFactory(IEventHandler handler)
        {
            Handler = handler;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            return new EventHandlerDisposeWrapper(Handler);
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories.Contains(this);
        }
    }

    public class MyOrderEventHandler : ILocalEventHandler<MyOrderEventHandlerEventData>, ITransientDependency
    {
        public Task HandleEventAsync(MyOrderEventHandlerEventData eventData)
        {
            HandlerExecuteOrder += "1";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-2)]
    public class MyOrderEventHandler2 : ILocalEventHandler<MyOrderEventHandlerEventData>, ITransientDependency
    {
        public Task HandleEventAsync(MyOrderEventHandlerEventData eventData)
        {
            HandlerExecuteOrder += "2";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-3)]
    public class MyOrderEventHandler3 : ILocalEventHandler<MyOrderEventHandlerEventData>, ITransientDependency
    {
        public Task HandleEventAsync(MyOrderEventHandlerEventData eventData)
        {
            HandlerExecuteOrder += "3";
            return Task.CompletedTask;
        }
    }

    [EventName("Volo.Abp.EventBus.Local.NamedOrderEvent")]
    public class NamedOrderEventData
    {
    }

    public class IocReplacedOrderEventData
    {
    }

    public class ConcurrentOrderEventData
    {
    }

    [LocalEventHandlerOrder(10)]
    public class LateTypedOrderEventHandler : ILocalEventHandler<NamedOrderEventData>
    {
        public Task HandleEventAsync(NamedOrderEventData eventData)
        {
            HandlerExecuteOrder += "T";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-10)]
    public class EarlyDynamicOrderEventHandler : ILocalEventHandler<DynamicEventData>
    {
        public Task HandleEventAsync(DynamicEventData eventData)
        {
            HandlerExecuteOrder += "D";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(10)]
    public class LateDynamicOrderEventHandler : ILocalEventHandler<DynamicEventData>
    {
        public Task HandleEventAsync(DynamicEventData eventData)
        {
            HandlerExecuteOrder += "L";
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Unsubscribes itself from an event name no event type maps to when its first handler is disposed.
    /// </summary>
    public class SelfUnsubscribingDynamicHandlerFactory : IEventHandlerFactory
    {
        private readonly ILocalEventBus _localEventBus;
        private readonly string _eventName;

        public SelfUnsubscribingDynamicHandlerFactory(ILocalEventBus localEventBus, string eventName)
        {
            _localEventBus = localEventBus;
            _eventName = eventName;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            return new EventHandlerDisposeWrapper(
                new EarlyDynamicOrderEventHandler(),
                () => _localEventBus.Unsubscribe(_eventName, this));
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories.Contains(this);
        }
    }

    [LocalEventHandlerOrder(10)]
    public class OriginalIocOrderEventHandler : ILocalEventHandler<IocReplacedOrderEventData>
    {
        public virtual Task HandleEventAsync(IocReplacedOrderEventData eventData)
        {
            HandlerExecuteOrder += "O";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-10)]
    public class ReplacementIocOrderEventHandler : OriginalIocOrderEventHandler, IDisposable
    {
        private static int _created;
        private static int _disposed;

        public static int Created => _created;

        public static int Disposed => _disposed;

        public ReplacementIocOrderEventHandler()
        {
            Interlocked.Increment(ref _created);
        }

        public static void Reset()
        {
            _created = 0;
            _disposed = 0;
        }

        public override Task HandleEventAsync(IocReplacedOrderEventData eventData)
        {
            HandlerExecuteOrder += "R";
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Interlocked.Increment(ref _disposed);
        }
    }

    public class ZeroOrderIocEventHandler : ILocalEventHandler<IocReplacedOrderEventData>
    {
        public Task HandleEventAsync(IocReplacedOrderEventData eventData)
        {
            HandlerExecuteOrder += "0";
            return Task.CompletedTask;
        }
    }

    public class ConcurrentOrderEventHandler : ILocalEventHandler<ConcurrentOrderEventData>
    {
        public Task HandleEventAsync(ConcurrentOrderEventData eventData)
        {
            return Task.CompletedTask;
        }
    }
}
