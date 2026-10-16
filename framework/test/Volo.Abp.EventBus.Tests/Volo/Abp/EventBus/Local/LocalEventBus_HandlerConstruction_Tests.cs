using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class LocalEventBus_HandlerConstruction_Tests : EventBusTestBase
{
    [Fact]
    public async Task A_Later_Handler_Should_Be_Created_After_An_Earlier_One_Ran()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ConstructionState>();
        services.AddTransient<StateWritingHandler>();
        services.AddTransient<StateReadingHandler>();
        await using var provider = services.BuildServiceProvider();
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

        using (LocalEventBus.Subscribe(typeof(StateEventData), new IocEventHandlerFactory(scopeFactory, typeof(StateReadingHandler))))
        using (LocalEventBus.Subscribe(typeof(StateEventData), new IocEventHandlerFactory(scopeFactory, typeof(StateWritingHandler))))
        {
            await LocalEventBus.PublishAsync(new StateEventData());
        }

        provider.GetRequiredService<ConstructionState>().SeenByReader.ShouldBe(42);
    }

    [Fact]
    public async Task A_Handler_Created_To_Read_Its_Order_Should_Not_Stop_The_Publish_When_It_Fails_To_Dispose()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ConstructionState>();
        services.AddTransient<DisposeFailingStateHandler>();
        await using var provider = services.BuildServiceProvider();

        using (LocalEventBus.Subscribe(typeof(StateEventData), new IocEventHandlerFactory(provider.GetRequiredService<IServiceScopeFactory>(), typeof(DisposeFailingStateHandler))))
        {
            // The instance that handled the event still fails to dispose afterwards, as it did before.
            await Should.ThrowAsync<System.InvalidOperationException>(() => LocalEventBus.PublishAsync(new StateEventData()));
        }

        provider.GetRequiredService<ConstructionState>().Value.ShouldBe(1);
    }

    [Fact]
    public async Task A_Handler_That_Fails_To_Resolve_While_Its_Order_Is_Read_Should_Still_Be_Disposed()
    {
        var factory = new LazilyFailingHandlerFactory();
        var eventName = "Volo.Abp.EventBus.Local.LocalEventBus_HandlerConstruction_Tests.LazilyFailing";
        using (LocalEventBus.Subscribe(eventName, factory))
        {
            (await Should.ThrowAsync<System.InvalidOperationException>(() => LocalEventBus.PublishAsync(eventName, new { }, onUnitOfWorkComplete: false)))
                .Message.ShouldBe("Lazy resolution failed.");
        }

        factory.Disposed.ShouldBe(1);
    }

    public class LazilyFailingHandlerFactory : IEventHandlerFactory
    {
        public int Disposed { get; private set; }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            return new LazilyFailingHandlerWrapper(this);
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories.Contains(this);
        }

        private class LazilyFailingHandlerWrapper : IEventHandlerDisposeWrapper
        {
            private readonly LazilyFailingHandlerFactory _factory;

            public LazilyFailingHandlerWrapper(LazilyFailingHandlerFactory factory)
            {
                _factory = factory;
            }

            public IEventHandler EventHandler => throw new System.InvalidOperationException("Lazy resolution failed.");

            public void Dispose()
            {
                _factory.Disposed++;
            }
        }
    }

    [Fact]
    public async Task A_Derived_Bus_Should_Choose_The_Handlers_Of_A_Mapped_Dynamic_Event()
    {
        var bus = ActivatorUtilities.CreateInstance<FilteringLocalEventBus>(ServiceProvider);
        var handled = 0;
        using (bus.Subscribe<FilteredEventData>(_ =>
        {
            handled++;
            return Task.CompletedTask;
        }))
        {
            await bus.TriggerHandlersAsync(typeof(DynamicEventData), new DynamicEventData(FilteredEventData.Name, new { }));
            await bus.PublishAsync(new FilteredEventData(), onUnitOfWorkComplete: false);
        }

        handled.ShouldBe(0);
        bus.FilteredCalls.ShouldBe(2);
    }

    [EventName(Name)]
    public class FilteredEventData
    {
        public const string Name = "Volo.Abp.EventBus.Local.LocalEventBus_HandlerConstruction_Tests.Filtered";
    }

    // Kept out of conventional registration, which would make it the application's bus.
    [DisableConventionalRegistration]
    public class FilteringLocalEventBus : LocalEventBus
    {
        public int FilteredCalls { get; private set; }

        public FilteringLocalEventBus(
            IOptions<AbpLocalEventBusOptions> options,
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(options, serviceScopeFactory, currentTenant, unitOfWorkManager, eventHandlerInvoker)
        {
        }

        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(System.Type eventType)
        {
            if (eventType == typeof(FilteredEventData))
            {
                FilteredCalls++;
                return Enumerable.Empty<EventTypeWithEventHandlerFactories>();
            }

            return base.GetHandlerFactories(eventType);
        }
    }
}

public class StateEventData
{
}

public class ConstructionState
{
    public int Value { get; set; }

    public int SeenByReader { get; set; } = -1;
}

public class DisposeFailingStateHandler : ILocalEventHandler<StateEventData>, System.IDisposable
{
    private readonly ConstructionState _state;

    public DisposeFailingStateHandler(ConstructionState state)
    {
        _state = state;
    }

    public Task HandleEventAsync(StateEventData eventData)
    {
        _state.Value++;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        throw new System.InvalidOperationException("Dispose failed.");
    }
}

[LocalEventHandlerOrder(-1)]
public class StateWritingHandler : ILocalEventHandler<StateEventData>
{
    private readonly ConstructionState _state;

    public StateWritingHandler(ConstructionState state)
    {
        _state = state;
    }

    public Task HandleEventAsync(StateEventData eventData)
    {
        _state.Value = 42;
        return Task.CompletedTask;
    }
}

[LocalEventHandlerOrder(1)]
public class StateReadingHandler : ILocalEventHandler<StateEventData>
{
    private readonly ConstructionState _state;
    private readonly int _valueWhenCreated;

    public StateReadingHandler(ConstructionState state)
    {
        _state = state;
        _valueWhenCreated = state.Value;
    }

    public Task HandleEventAsync(StateEventData eventData)
    {
        _state.SeenByReader = _valueWhenCreated;
        return Task.CompletedTask;
    }
}
