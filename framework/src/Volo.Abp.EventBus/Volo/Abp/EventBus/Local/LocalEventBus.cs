using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Local;

/// <summary>
/// Implements EventBus as Singleton pattern.
/// </summary>
[ExposeServices(typeof(ILocalEventBus), typeof(LocalEventBus))]
public class LocalEventBus : EventBusBase, ILocalEventBus, ISingletonDependency
{
    /// <summary>
    /// Reference to the Logger.
    /// </summary>
    public ILogger<LocalEventBus> Logger { get; set; }

    protected AbpLocalEventBusOptions Options { get; }

    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }

    protected ConcurrentDictionary<string, Type> EventTypes { get; }

    protected ConcurrentDictionary<string, List<IEventHandlerFactory>> DynamicEventHandlerFactories { get; }

    public LocalEventBus(
        IOptions<AbpLocalEventBusOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IEventHandlerInvoker eventHandlerInvoker)
        : base(serviceScopeFactory, currentTenant, unitOfWorkManager, eventHandlerInvoker)
    {
        Options = options.Value;
        Logger = NullLogger<LocalEventBus>.Instance;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        DynamicEventHandlerFactories = new ConcurrentDictionary<string, List<IEventHandlerFactory>>();
        SubscribeHandlers(Options.Handlers);
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories =>
        {
            if (!handler.IsInFactories(factories))
            {
                factories.Add(handler);
            }
        });

        return new DynamicEventHandlerFactoryUnregistrar(this, eventName, handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        EventTypes.GetOrAdd(EventNameAttribute.GetNameOrDefault(eventType), eventType);

        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
                {
                    if (!factory.IsInFactories(factories))
                    {
                        factories.Add(factory);
                    }
                }
            );

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
    public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
    {
        Check.NotNull(action, nameof(action));

        GetOrCreateHandlerFactories(typeof(TEvent))
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                    {
                        var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                        if (singleInstanceFactory == null)
                        {
                            return false;
                        }

                        var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEvent>;
                        if (actionHandler == null)
                        {
                            return false;
                        }

                        return actionHandler.Action == action;
                    });
            });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory &&
                        ((factory as SingleInstanceHandlerFactory)!).HandlerInstance == handler
                );
            });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandlerFactory factory)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandler handler)
    {
        GetOrCreateDynamicHandlerFactories(eventName)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory singleFactory &&
                        singleFactory.HandlerInstance == handler
                );
            });
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(string eventName)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories => factories.Clear());
    }

    /// <inheritdoc/>
    public override Task PublishAsync(string eventName, object eventData, bool onUnitOfWorkComplete = true)
    {
        var eventType = EventTypes.GetOrDefault(eventName);

        var dynamicEventData = eventData as DynamicEventData ?? new DynamicEventData(eventName, eventData);

        if (eventType != null)
        {
            return PublishAsync(eventType, ConvertDynamicEventData(dynamicEventData.Data, eventType), onUnitOfWorkComplete);
        }

        return PublishAsync(typeof(DynamicEventData), dynamicEventData, onUnitOfWorkComplete);
    }

    protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(new LocalEventMessage(Guid.NewGuid(), eventData, eventType));
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceLocalEvent(eventRecord);
    }

    public virtual async Task PublishAsync(LocalEventMessage localEventMessage)
    {
        await TriggerHandlersAsync(localEventMessage.EventType, localEventMessage.EventData);
    }

    public virtual List<EventTypeWithEventHandlerFactories> GetEventHandlerFactories(Type eventType)
    {
        return GetHandlerFactories(eventType).ToList();
    }

    /// <inheritdoc/>
    public virtual List<EventTypeWithEventHandlerFactories> GetDynamicEventHandlerFactories(string eventName)
    {
        return GetDynamicHandlerFactories(eventName).ToList();
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();
        var eventNames = EventTypes.Where(x => ShouldTriggerEventForHandler(eventType, x.Value)).Select(x => x.Key).ToList();

        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            foreach (var factory in handlerFactory.Value.Locking(factories => factories.ToList()))
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(factory, handlerFactory.Key, GetHandlerOrder(factory)));
            }
        }

        foreach (var handlerFactory in DynamicEventHandlerFactories.Where(aehf => eventNames.Contains(aehf.Key)))
        {
            foreach (var factory in handlerFactory.Value.Locking(factories => factories.ToList()))
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(factory, typeof(DynamicEventData), GetHandlerOrder(factory)));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x => new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> {x.Item1})).ToArray();
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetDynamicHandlerFactories(string eventName)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType != null)
        {
            return GetHandlerFactories(eventType);
        }

        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();

        foreach (var handlerFactory in DynamicEventHandlerFactories.Where(aehf => aehf.Key == eventName))
        {
            foreach (var factory in handlerFactory.Value.Locking(factories => factories.ToList()))
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(factory, typeof(DynamicEventData), GetHandlerOrder(factory)));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x =>
            new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> { x.Item1 })).ToArray();
    }

    private static readonly ConditionalWeakTable<Type, StrongBox<int>> HandlerOrders = new();

    /// <summary>
    /// Disposes a handler the bus created for itself, so that a failure there neither fails the publish nor replaces
    /// the exception that failed it.
    /// </summary>
    private void DisposeQuietly(IDisposable disposable, string message)
    {
        try
        {
            disposable.Dispose();
        }
        catch (Exception ex)
        {
            try
            {
                Logger.LogWarning(ex, message);
            }
            catch
            {
                // A failing logger must not replace the exception that failed the publish.
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="LocalEventHandlerOrderAttribute"/> order of the handler the factory creates.
    /// When the handler type is known without creating a handler (the built-in factories of a fixed type, and an
    /// IoC handler whose registration in the container fixes its type), the order is read from that type.
    /// Otherwise the handler is created to read its type and is disposed at once, on each publish. Callers
    /// enumerate a snapshot of the factories, since that dispose runs handler code which may unsubscribe.
    /// </summary>
    protected virtual int GetHandlerOrder(IEventHandlerFactory factory)
    {
        var handlerType = GetFixedHandlerTypeOrNull(factory);
        if (handlerType == null)
        {
            var handler = factory.GetHandler();
            try
            {
                handlerType = handler.EventHandler.GetType();
            }
            finally
            {
                DisposeQuietly(handler, "Could not dispose an event handler created to read its order.");
            }
        }

        return GetHandlerOrderOfType(handlerType);
    }

    /// <summary>
    /// The order is read from a type's attributes once and kept with the type, which may still be unloaded.
    /// </summary>
    private static int GetHandlerOrderOfType(Type handlerType)
    {
        return HandlerOrders.GetValue(handlerType, static type => new StrongBox<int>(ReflectionHelper
            .GetAttributesOfMemberOrDeclaringType<LocalEventHandlerOrderAttribute>(type)
            .FirstOrDefault()?.Order ?? 0)).Value;
    }

    private static Type? GetFixedHandlerTypeOrNull(IEventHandlerFactory factory)
    {
        var factoryType = factory.GetType();

        if (factoryType == typeof(SingleInstanceHandlerFactory))
        {
            return ((SingleInstanceHandlerFactory)factory).HandlerInstance.GetType();
        }

        // Only the built-in transient factories: a derived one may override how the handler is created.
        if (factoryType == typeof(TransientEventHandlerFactory) ||
            factoryType.IsGenericType && factoryType.GetGenericTypeDefinition() == typeof(TransientEventHandlerFactory<>))
        {
            return ((TransientEventHandlerFactory)factory).HandlerType;
        }

        // Only the built-in IoC factory, for the same reason.
        if (factoryType == typeof(IocEventHandlerFactory))
        {
            return ((IocEventHandlerFactory)factory).GetImplementationTypeOrNull();
        }

        return null;
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
    }

    private List<IEventHandlerFactory> GetOrCreateDynamicHandlerFactories(string eventName)
    {
        return DynamicEventHandlerFactories.GetOrAdd(eventName, (name) => new List<IEventHandlerFactory>());
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        if (handlerEventType == targetEventType)
        {
            return true;
        }

        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}
