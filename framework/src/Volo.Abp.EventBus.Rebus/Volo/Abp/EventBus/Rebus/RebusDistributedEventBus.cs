﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Bus;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Rebus
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDistributedEventBus), typeof(RebusDistributedEventBus))]
    public class RebusDistributedEventBus : EventBusBase, IDistributedEventBus, ISingletonDependency
    {
        protected IBus Rebus { get; }

        //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }
        protected AbpRebusEventBusOptions AbpRebusEventBusOptions { get; }

        public RebusDistributedEventBus(
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IBus rebus,
            IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
            IOptions<AbpRebusEventBusOptions> abpEventBusRebusOptions,
            IEventErrorHandler errorHandler) :
            base(serviceScopeFactory, currentTenant, errorHandler)
        {
            Rebus = rebus;
            AbpRebusEventBusOptions = abpEventBusRebusOptions.Value;
            AbpDistributedEventBusOptions = abpDistributedEventBusOptions.Value;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
        }

        public void Initialize()
        {
            SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);

            if (factory.IsInFactories(handlerFactories))
            {
                return NullDisposable.Instance;
            }

            handlerFactories.Add(factory);

            if (handlerFactories.Count == 1) //TODO: Multi-threading!
            {
                Rebus.Subscribe(eventType);
            }

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEvent))
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                        {
                            if (!(factory is SingleInstanceHandlerFactory singleInstanceFactory))
                            {
                                return false;
                            }

                            if (!(singleInstanceFactory.HandlerInstance is ActionEventHandler<TEvent> actionHandler))
                            {
                                return false;
                            }

                            return actionHandler.Action == action;
                        });
                });

            Rebus.Unsubscribe(typeof(TEvent));
        }

        public override void Unsubscribe(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                            factory is SingleInstanceHandlerFactory handlerFactory &&
                            handlerFactory.HandlerInstance == handler
                    );
                });

            Rebus.Unsubscribe(eventType);
        }

        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
            Rebus.Unsubscribe(eventType);
        }

        public override void UnsubscribeAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
            Rebus.Unsubscribe(eventType);
        }

        public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        public override async Task PublishAsync(Type eventType, object eventData)
        {
            await AbpRebusEventBusOptions.Publish(Rebus, eventType, eventData);
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetNameOrDefault(type);
                    EventTypes[eventName] = type;
                    return new List<IEventHandlerFactory>();
                }
            );
        }

        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key))
            )
            {
                handlerFactoryList.Add(
                    new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
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
