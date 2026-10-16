using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EventBus.Local;

/// <summary>
/// An IoC handler whose registration tells its type is ordered by that type, so it is created only to handle the event.
/// </summary>
public class LocalEventBus_RegisteredHandlerType_Tests : EventBusTestBase
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        // Registered with Autofac directly: registered through the service collection, they would also be subscribed
        // to the application's bus by convention.
        var builder = services.GetObject<ContainerBuilder>();
        builder.RegisterType<Tracker>().SingleInstance();
        builder.RegisterType<TrackedDependency>().InstancePerLifetimeScope();
        builder.RegisterType<ConstructionState>().SingleInstance();
        builder.RegisterType<FirstHandler>();
        builder.RegisterType<SecondHandler>();
        builder.RegisterType<ReplacementOfReplacedHandler>().As<ReplacedHandler>();
        builder.Register(context => context.Resolve<Tracker>().CreateReplacement
                ? new ReplacementOfSwitchedHandler(context.Resolve<TrackedDependency>())
                : new SwitchedHandler(context.Resolve<TrackedDependency>()))
            .As<SwitchedHandler>();
        builder.RegisterType<ActivatedHandler>()
            .OnActivating(e => e.ReplaceInstance(new ReplacementOfActivatedHandler(e.Context.Resolve<TrackedDependency>())));
        builder.RegisterType<ThrowingWhileHandlingHandler>();
        builder.RegisterType<ConstructionFailingHandler>();
        builder.RegisterType<DynamicHandler>();
        builder.RegisterType<OuterHandler>();
        builder.RegisterType<InnerHandler>();
        builder.RegisterType<NamedDynamicHandler>();
        builder.RegisterType<NamedTypedHandler>();
        builder.RegisterType<StateWritingHandler>();
        builder.RegisterType<StateReadingHandler>();

        services.OnRegistered(context =>
        {
            if (context.ImplementationType == typeof(InterceptedHandler))
            {
                context.Interceptors.TryAdd<CountingInterceptor>();
            }
        });
    }

    [Fact]
    public async Task An_Ioc_Handler_Should_Be_Created_Once_Per_Publish()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, SecondHandler>())
        {
            for (var i = 0; i < 3; i++)
            {
                await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
            }
        }

        tracker.Log.Count.ShouldBe(3);
        tracker.Created.ShouldBe(3);
        tracker.Disposed.ShouldBe(3);
    }

    [Fact]
    public async Task A_Replaced_Ioc_Handler_Should_Be_Ordered_By_The_Replacement_And_Created_Once()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, FirstHandler>())
        using (Subscribe<RegisteredTypeEventData, ReplacedHandler>())
        {
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
        }

        tracker.Log.ShouldBe(new[] { "replacement", "first disposed=1" });
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    [Fact]
    public async Task A_Handler_Registered_With_A_Factory_Method_Should_Be_Ordered_By_What_It_Creates_On_Each_Publish()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, FirstHandler>())
        using (Subscribe<RegisteredTypeEventData, SwitchedHandler>())
        {
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
            tracker.Log.ShouldBe(new[] { "first disposed=1", "switched" });

            tracker.CreateReplacement = true;
            tracker.Log.Clear();
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
            tracker.Log.ShouldBe(new[] { "switched replacement", "first disposed=5" });
        }

        // Its type can only be read from a handler, which is created to read it on each publish.
        tracker.Created.ShouldBe(6);
        tracker.Disposed.ShouldBe(6);
    }

    [Fact]
    public async Task A_Handler_Replaced_When_Activated_Should_Be_Ordered_By_What_Is_Created_From_Then_On()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, FirstHandler>())
        using (Subscribe<RegisteredTypeEventData, ActivatedHandler>())
        {
            // The registration can not tell the replacement, so the first publish orders by the registered type.
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
            tracker.Log.ShouldBe(new[] { "first disposed=0", "activated replacement" });

            // Having created another type, the factory reads the type from a handler from then on.
            tracker.Log.Clear();
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
            tracker.Log.ShouldBe(new[] { "activated replacement", "first disposed=4" });
        }
    }

    [Fact]
    public async Task A_Later_Handler_Should_Be_Created_After_An_Earlier_One_Ran()
    {
        var state = GetRequiredService<ConstructionState>();
        using (Subscribe<StateEventData, StateReadingHandler>())
        using (Subscribe<StateEventData, StateWritingHandler>())
        {
            for (var i = 0; i < 2; i++)
            {
                state.Value = 0;
                state.SeenByReader = -1;

                await LocalEventBus.PublishAsync(new StateEventData());

                state.SeenByReader.ShouldBe(42);
            }
        }
    }

    [Fact]
    public async Task Each_Handler_Should_Release_Its_Scope_Before_The_Next_One_Runs()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, SecondHandler>())
        using (Subscribe<RegisteredTypeEventData, FirstHandler>())
        {
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
        }

        tracker.Log.ShouldBe(new[] { "first disposed=0", "second disposed=1" });
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    [Fact]
    public async Task A_Handler_That_Throws_Should_Not_Stop_The_Next_One_Or_Keep_Any_Scope()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, SecondHandler>())
        using (Subscribe<RegisteredTypeEventData, ThrowingWhileHandlingHandler>())
        {
            (await Should.ThrowAsync<InvalidOperationException>(() => LocalEventBus.PublishAsync(new RegisteredTypeEventData())))
                .Message.ShouldBe("Handling failed.");
        }

        tracker.Log.ShouldBe(new[] { "throwing", "second disposed=1" });
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    [Fact]
    public async Task A_Handler_That_Fails_To_Construct_Should_Stop_The_Publish_After_The_Earlier_Ones_Ran()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, FirstHandler>())
        using (Subscribe<RegisteredTypeEventData, ConstructionFailingHandler>())
        using (Subscribe<RegisteredTypeEventData, SecondHandler>())
        {
            (await Should.ThrowAsync<Exception>(() => LocalEventBus.PublishAsync(new RegisteredTypeEventData())))
                .GetBaseException().Message.ShouldBe("Construction failed.");
        }

        tracker.Log.ShouldBe(new[] { "first disposed=0" });
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    [Fact]
    public async Task A_Dynamic_Ioc_Handler_Should_Be_Created_Once_Per_Publish()
    {
        var tracker = GetRequiredService<Tracker>();
        var eventName = "RegisteredTypeDynamic-" + Guid.NewGuid().ToString("N");
        using (LocalEventBus.Subscribe(eventName, CreateFactory<DynamicHandler>()))
        {
            await LocalEventBus.PublishAsync(eventName, new { Value = 1 });
        }

        tracker.Log.ShouldBe(new[] { "dynamic" });
        tracker.Created.ShouldBe(1);
        tracker.Disposed.ShouldBe(1);
    }

    [Fact]
    public async Task A_Publish_Stopped_Before_A_Handler_Should_Not_Create_It()
    {
        var tracker = GetRequiredService<Tracker>();
        using (LocalEventBus.Subscribe(typeof(NamedEventData), CreateFactory<NamedTypedHandler>()))
        using (LocalEventBus.Subscribe(NamedEventData.Name, CreateFactory<NamedDynamicHandler>()))
        {
            // The typed handler's data cannot be converted, which ends the publish after the dynamic handler ran.
            await Should.ThrowAsync<Exception>(() => ((LocalEventBus)LocalEventBus).TriggerHandlersAsync(
                typeof(DynamicEventData),
                new DynamicEventData(NamedEventData.Name, new { Value = "not a number" })));
        }

        tracker.Log.ShouldBe(new[] { "named dynamic" });
        tracker.Created.ShouldBe(1);
        tracker.Disposed.ShouldBe(1);
    }

    [Fact]
    public async Task A_Publish_From_A_Handler_Should_Create_And_Release_Its_Own_Handlers()
    {
        var tracker = GetRequiredService<Tracker>();
        using (Subscribe<RegisteredTypeEventData, OuterHandler>())
        using (Subscribe<InnerEventData, InnerHandler>())
        {
            await LocalEventBus.PublishAsync(new RegisteredTypeEventData());
        }

        tracker.Log.ShouldBe(new[] { "inner disposed=0", "outer done" });
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    [Fact]
    public async Task An_Intercepted_Handler_Should_Be_Created_Once_Per_Publish()
    {
        InterceptedHandler.Reset();
        CountingInterceptor.Reset();

        for (var i = 0; i < 2; i++)
        {
            await LocalEventBus.PublishAsync(new InterceptedEventData());
        }

        // The registration creates a class proxy, the type every handler it creates has.
        InterceptedHandler.Created.ShouldBe(2);
        InterceptedHandler.Handled.ShouldBe(2);
        CountingInterceptor.Intercepted.ShouldBe(2);
    }

    [Fact]
    public async Task A_Derived_Bus_Should_Order_By_The_Registered_Type_Too()
    {
        var tracker = GetRequiredService<Tracker>();
        var derived = ActivatorUtilities.CreateInstance<DerivedLocalEventBus>(ServiceProvider);
        using (derived.Subscribe(typeof(RegisteredTypeEventData), CreateFactory<FirstHandler>()))
        using (derived.Subscribe(typeof(RegisteredTypeEventData), CreateFactory<ReplacedHandler>()))
        {
            await derived.PublishAsync(new RegisteredTypeEventData(), onUnitOfWorkComplete: false);
        }

        tracker.Log.ShouldBe(new[] { "replacement", "first disposed=1" });
        tracker.Created.ShouldBe(2);
        tracker.Disposed.ShouldBe(2);
    }

    private IDisposable Subscribe<TEvent, THandler>()
    {
        return LocalEventBus.Subscribe(typeof(TEvent), CreateFactory<THandler>());
    }

    private IocEventHandlerFactory CreateFactory<THandler>()
    {
        return new IocEventHandlerFactory(GetRequiredService<IServiceScopeFactory>(), typeof(THandler));
    }

    public class RegisteredTypeEventData
    {
    }

    public class InnerEventData
    {
    }

    public class InterceptedEventData
    {
    }

    [EventName(Name)]
    public class NamedEventData
    {
        public const string Name = "Volo.Abp.EventBus.Local.LocalEventBus_RegisteredHandlerType_Tests.Named";

        public int Value { get; set; }
    }

    public class Tracker
    {
        private int _created;
        private int _disposed;

        public List<string> Log { get; } = new();

        public bool CreateReplacement { get; set; }

        public int Created => _created;

        public int Disposed => _disposed;

        public void OnCreated() => Interlocked.Increment(ref _created);

        public void OnDisposed() => Interlocked.Increment(ref _disposed);
    }

    public class TrackedDependency : IDisposable
    {
        public Tracker Tracker { get; }

        public TrackedDependency(Tracker tracker)
        {
            Tracker = tracker;
            Tracker.OnCreated();
        }

        public void Dispose()
        {
            Tracker.OnDisposed();
        }
    }

    public abstract class TrackedHandler : ILocalEventHandler<RegisteredTypeEventData>
    {
        protected TrackedDependency Dependency { get; }

        protected TrackedHandler(TrackedDependency dependency)
        {
            Dependency = dependency;
        }

        public virtual Task HandleEventAsync(RegisteredTypeEventData eventData)
        {
            Dependency.Tracker.Log.Add(GetLogEntry());
            return Task.CompletedTask;
        }

        protected abstract string GetLogEntry();
    }

    [LocalEventHandlerOrder(-1)]
    public class FirstHandler : TrackedHandler
    {
        public FirstHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => $"first disposed={Dependency.Tracker.Disposed}";
    }

    [LocalEventHandlerOrder(1)]
    public class SecondHandler : TrackedHandler
    {
        public SecondHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => $"second disposed={Dependency.Tracker.Disposed}";
    }

    [LocalEventHandlerOrder(1)]
    public class ReplacedHandler : TrackedHandler
    {
        public ReplacedHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => "replaced";
    }

    [LocalEventHandlerOrder(-10)]
    public class ReplacementOfReplacedHandler : ReplacedHandler
    {
        public ReplacementOfReplacedHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => "replacement";
    }

    [LocalEventHandlerOrder(1)]
    public class SwitchedHandler : TrackedHandler
    {
        public SwitchedHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => "switched";
    }

    [LocalEventHandlerOrder(-10)]
    public class ReplacementOfSwitchedHandler : SwitchedHandler
    {
        public ReplacementOfSwitchedHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => "switched replacement";
    }

    [LocalEventHandlerOrder(1)]
    public class ActivatedHandler : TrackedHandler
    {
        public ActivatedHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => "activated";
    }

    [LocalEventHandlerOrder(-10)]
    public class ReplacementOfActivatedHandler : ActivatedHandler
    {
        public ReplacementOfActivatedHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        protected override string GetLogEntry() => "activated replacement";
    }

    [LocalEventHandlerOrder(-1)]
    public class ThrowingWhileHandlingHandler : TrackedHandler
    {
        public ThrowingWhileHandlingHandler(TrackedDependency dependency)
            : base(dependency)
        {
        }

        public override Task HandleEventAsync(RegisteredTypeEventData eventData)
        {
            base.HandleEventAsync(eventData);
            throw new InvalidOperationException("Handling failed.");
        }

        protected override string GetLogEntry() => "throwing";
    }

    public class ConstructionFailingHandler : ILocalEventHandler<RegisteredTypeEventData>
    {
        public ConstructionFailingHandler(TrackedDependency dependency)
        {
            throw new InvalidOperationException("Construction failed.");
        }

        public Task HandleEventAsync(RegisteredTypeEventData eventData)
        {
            return Task.CompletedTask;
        }
    }

    public class DynamicHandler : ILocalEventHandler<DynamicEventData>
    {
        private readonly TrackedDependency _dependency;

        public DynamicHandler(TrackedDependency dependency)
        {
            _dependency = dependency;
        }

        public Task HandleEventAsync(DynamicEventData eventData)
        {
            _dependency.Tracker.Log.Add("dynamic");
            return Task.CompletedTask;
        }
    }

    public class OuterHandler : ILocalEventHandler<RegisteredTypeEventData>
    {
        private readonly TrackedDependency _dependency;
        private readonly ILocalEventBus _localEventBus;

        public OuterHandler(TrackedDependency dependency, ILocalEventBus localEventBus)
        {
            _dependency = dependency;
            _localEventBus = localEventBus;
        }

        public async Task HandleEventAsync(RegisteredTypeEventData eventData)
        {
            await _localEventBus.PublishAsync(new InnerEventData(), onUnitOfWorkComplete: false);
            _dependency.Tracker.Log.Add("outer done");
        }
    }

    public class InnerHandler : ILocalEventHandler<InnerEventData>
    {
        private readonly TrackedDependency _dependency;

        public InnerHandler(TrackedDependency dependency)
        {
            _dependency = dependency;
        }

        public Task HandleEventAsync(InnerEventData eventData)
        {
            _dependency.Tracker.Log.Add($"inner disposed={_dependency.Tracker.Disposed}");
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-1)]
    public class NamedDynamicHandler : ILocalEventHandler<DynamicEventData>
    {
        private readonly TrackedDependency _dependency;

        public NamedDynamicHandler(TrackedDependency dependency)
        {
            _dependency = dependency;
        }

        public Task HandleEventAsync(DynamicEventData eventData)
        {
            _dependency.Tracker.Log.Add("named dynamic");
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(1)]
    public class NamedTypedHandler : ILocalEventHandler<NamedEventData>
    {
        private readonly TrackedDependency _dependency;

        public NamedTypedHandler(TrackedDependency dependency)
        {
            _dependency = dependency;
        }

        public Task HandleEventAsync(NamedEventData eventData)
        {
            _dependency.Tracker.Log.Add("named typed");
            return Task.CompletedTask;
        }
    }

    public class InterceptedHandler : ILocalEventHandler<InterceptedEventData>, ITransientDependency
    {
        private static int _created;
        private static int _handled;

        public static int Created => _created;

        public static int Handled => _handled;

        public InterceptedHandler()
        {
            Interlocked.Increment(ref _created);
        }

        public static void Reset()
        {
            _created = 0;
            _handled = 0;
        }

        public virtual Task HandleEventAsync(InterceptedEventData eventData)
        {
            Interlocked.Increment(ref _handled);
            return Task.CompletedTask;
        }
    }

    public class CountingInterceptor : AbpInterceptor, ITransientDependency
    {
        private static int _intercepted;

        public static int Intercepted => _intercepted;

        public static void Reset()
        {
            _intercepted = 0;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            Interlocked.Increment(ref _intercepted);
            await invocation.ProceedAsync();
        }
    }

    // Kept out of conventional registration, which would make it the application's bus.
    [DisableConventionalRegistration]
    public class DerivedLocalEventBus : LocalEventBus
    {
        public DerivedLocalEventBus(
            IOptions<AbpLocalEventBusOptions> options,
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IEventHandlerInvoker eventHandlerInvoker)
            : base(options, serviceScopeFactory, currentTenant, unitOfWorkManager, eventHandlerInvoker)
        {
        }
    }
}
