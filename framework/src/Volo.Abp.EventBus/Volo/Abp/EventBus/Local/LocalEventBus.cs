using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Local
{
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

        public LocalEventBus(
            IOptions<AbpLocalEventBusOptions> options,
            IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory)
        {
            Options = options.Value;
            Logger = NullLogger<LocalEventBus>.Instance;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            SubscribeHandlers(Options.Handlers);
        }

        /// <inheritdoc/>
        public virtual IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        /// <inheritdoc/>
        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
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
                            (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                    );
                });
        }

        /// <inheritdoc/>
        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        /// <inheritdoc/>
        public override void UnsubscribeAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
        }

        public override async Task PublishAsync(Type eventType, object eventData)
        {
            var exceptions = new List<Exception>();

            await TriggerHandlersAsync(eventType, eventData, exceptions);

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException("More than one error has occurred while triggering the event: " + eventType, exceptions);
            }
        }

        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
            {
                return true;
            }

            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }
    }
}