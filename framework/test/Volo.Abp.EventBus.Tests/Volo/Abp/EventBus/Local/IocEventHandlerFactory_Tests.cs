using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class IocEventHandlerFactory_Tests : EventBusTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        AddThrowingHandler(services);
    }

    [Fact]
    public void A_Handler_That_Fails_To_Construct_Should_Not_Leave_Its_Scope_Undisposed_With_Autofac()
    {
        var tracker = GetRequiredService<ScopeTracker>();
        var factory = new IocEventHandlerFactory(GetRequiredService<IServiceScopeFactory>(), typeof(ThrowingHandler));

        Should.Throw<Exception>(() => factory.GetHandler());

        tracker.Created.ShouldBe(1);
        tracker.Disposed.ShouldBe(1);
    }

    [Fact]
    public void A_Handler_That_Fails_To_Construct_Should_Not_Leave_Its_Scope_Undisposed_With_Microsoft_DI()
    {
        var services = new ServiceCollection();
        AddThrowingHandler(services);
        using var provider = services.BuildServiceProvider();
        var tracker = provider.GetRequiredService<ScopeTracker>();
        var factory = new IocEventHandlerFactory(provider.GetRequiredService<IServiceScopeFactory>(), typeof(ThrowingHandler));

        Should.Throw<InvalidOperationException>(() => factory.GetHandler()).Message.ShouldBe("Handler construction failed.");

        tracker.Created.ShouldBe(1);
        tracker.Disposed.ShouldBe(1);
    }

    [Fact]
    public async Task Publishing_To_A_Handler_That_Fails_To_Construct_Should_Dispose_Every_Scope_It_Opened()
    {
        var tracker = GetRequiredService<ScopeTracker>();
        using (LocalEventBus.Subscribe(typeof(ThrowingHandlerEventData), new IocEventHandlerFactory(GetRequiredService<IServiceScopeFactory>(), typeof(ThrowingHandler))))
        {
            await Should.ThrowAsync<Exception>(() => LocalEventBus.PublishAsync(new ThrowingHandlerEventData()));
        }

        tracker.Created.ShouldBeGreaterThan(0);
        tracker.Disposed.ShouldBe(tracker.Created);
    }

    [Fact]
    public void A_Handler_That_Fails_To_Construct_Should_Report_Its_Own_Exception_When_Its_Scope_Fails_To_Dispose()
    {
        var services = new ServiceCollection();
        services.AddScoped<DisposeFailingDependency>();
        services.AddTransient<HandlerWithDisposeFailingDependency>();
        using var provider = services.BuildServiceProvider();
        var factory = new IocEventHandlerFactory(provider.GetRequiredService<IServiceScopeFactory>(), typeof(HandlerWithDisposeFailingDependency));

        Should.Throw<InvalidOperationException>(() => factory.GetHandler()).Message.ShouldBe("Handler construction failed.");
    }

    [Fact]
    public void A_Resolved_Handler_Should_Keep_Its_Scope_Until_It_Is_Disposed()
    {
        var tracker = GetRequiredService<ScopeTracker>();
        var factory = new IocEventHandlerFactory(GetRequiredService<IServiceScopeFactory>(), typeof(WorkingHandler));

        var wrapper = factory.GetHandler();
        wrapper.EventHandler.ShouldBeOfType<WorkingHandler>().Dependency.IsDisposed.ShouldBeFalse();
        tracker.Created.ShouldBe(1);
        tracker.Disposed.ShouldBe(0);

        wrapper.Dispose();
        tracker.Disposed.ShouldBe(1);
    }

    [Fact]
    public async Task A_Handler_That_Fails_To_Construct_While_Ordering_Should_Dispose_Both_Without_Invoking_Either()
    {
        // A Microsoft container of its own: it does not tell the handler types, so both are created to read their
        // order, and neither is also subscribed to the application's bus by convention.
        var services = new ServiceCollection();
        AddThrowingHandler(services);
        services.AddTransient<EarlierWorkingHandler>();
        services.AddTransient<LaterThrowingHandler>();
        await using var provider = services.BuildServiceProvider();
        var tracker = provider.GetRequiredService<ScopeTracker>();
        var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        using (LocalEventBus.Subscribe(typeof(OrderingFailureEventData), new IocEventHandlerFactory(scopeFactory, typeof(EarlierWorkingHandler))))
        using (LocalEventBus.Subscribe(typeof(OrderingFailureEventData), new IocEventHandlerFactory(scopeFactory, typeof(LaterThrowingHandler))))
        {
            await Should.ThrowAsync<Exception>(() => LocalEventBus.PublishAsync(new OrderingFailureEventData()));
        }

        tracker.Handled.ShouldBe(0);
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    private static void AddThrowingHandler(IServiceCollection services)
    {
        services.AddSingleton<ScopeTracker>();
        services.AddScoped<ScopedDependency>();
        services.AddTransient<ThrowingHandler>();
        services.AddTransient<WorkingHandler>();
    }

    public class ThrowingHandlerEventData
    {
    }

    public class OrderingFailureEventData
    {
    }

    public class DisposeFailingDependency : IDisposable
    {
        public void Dispose()
        {
            throw new ApplicationException("Scope disposal failed.");
        }
    }

    public class HandlerWithDisposeFailingDependency : ILocalEventHandler<OrderingFailureEventData>
    {
        public HandlerWithDisposeFailingDependency(DisposeFailingDependency dependency)
        {
            throw new InvalidOperationException("Handler construction failed.");
        }

        public Task HandleEventAsync(OrderingFailureEventData eventData)
        {
            return Task.CompletedTask;
        }
    }

    public class ScopeTracker
    {
        private int _created;
        private int _disposed;
        private int _handled;

        public int Created => _created;

        public int Disposed => _disposed;

        public int Handled => _handled;

        public void OnHandled() => Interlocked.Increment(ref _handled);

        public void OnCreated() => Interlocked.Increment(ref _created);

        public void OnDisposed() => Interlocked.Increment(ref _disposed);
    }

    public class ScopedDependency : IDisposable
    {
        private readonly ScopeTracker _tracker;

        public ScopeTracker Tracker => _tracker;

        public bool IsDisposed { get; private set; }

        public ScopedDependency(ScopeTracker tracker)
        {
            _tracker = tracker;
            _tracker.OnCreated();
        }

        public void Dispose()
        {
            IsDisposed = true;
            _tracker.OnDisposed();
        }
    }

    public class ThrowingHandler : ILocalEventHandler<ThrowingHandlerEventData>
    {
        public ThrowingHandler(ScopedDependency dependency)
        {
            throw new InvalidOperationException("Handler construction failed.");
        }

        public Task HandleEventAsync(ThrowingHandlerEventData eventData)
        {
            return Task.CompletedTask;
        }
    }

    public class WorkingHandler : ILocalEventHandler<ThrowingHandlerEventData>
    {
        public ScopedDependency Dependency { get; }

        public WorkingHandler(ScopedDependency dependency)
        {
            Dependency = dependency;
        }

        public Task HandleEventAsync(ThrowingHandlerEventData eventData)
        {
            Dependency.Tracker.OnHandled();
            return Task.CompletedTask;
        }
    }

    public class EarlierWorkingHandler : ILocalEventHandler<OrderingFailureEventData>
    {
        private readonly ScopedDependency _dependency;

        public EarlierWorkingHandler(ScopedDependency dependency)
        {
            _dependency = dependency;
        }

        public Task HandleEventAsync(OrderingFailureEventData eventData)
        {
            _dependency.Tracker.OnHandled();
            return Task.CompletedTask;
        }
    }

    public class LaterThrowingHandler : ILocalEventHandler<OrderingFailureEventData>
    {
        public LaterThrowingHandler(ScopedDependency dependency)
        {
            throw new InvalidOperationException("Handler construction failed.");
        }

        public Task HandleEventAsync(OrderingFailureEventData eventData)
        {
            return Task.CompletedTask;
        }
    }
}
