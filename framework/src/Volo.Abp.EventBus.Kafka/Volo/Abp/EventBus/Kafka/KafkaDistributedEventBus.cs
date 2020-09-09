﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Kafka;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Kafka
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDistributedEventBus), typeof(KafkaDistributedEventBus))]
    public class KafkaDistributedEventBus : EventBusBase, IDistributedEventBus, ISingletonDependency
    {
        protected AbpKafkaEventBusOptions AbpKafkaEventBusOptions { get; }
        protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }
        protected IKafkaMessageConsumerFactory MessageConsumerFactory { get; }
        protected IKafkaSerializer Serializer { get; }
        protected IProducerPool ProducerPool { get; }
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected IKafkaMessageConsumer Consumer { get; private set; }

        public KafkaDistributedEventBus(
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant,
            IOptions<AbpKafkaEventBusOptions> abpKafkaEventBusOptions,
            IKafkaMessageConsumerFactory messageConsumerFactory,
            IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
            IKafkaSerializer serializer,
            IProducerPool producerPool)
            : base(serviceScopeFactory, currentTenant)
        {
            AbpKafkaEventBusOptions = abpKafkaEventBusOptions.Value;
            AbpDistributedEventBusOptions = abpDistributedEventBusOptions.Value;
            MessageConsumerFactory = messageConsumerFactory;
            Serializer = serializer;
            ProducerPool = producerPool;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
        }

        public void Initialize()
        {
            Consumer = MessageConsumerFactory.Create(
                AbpKafkaEventBusOptions.TopicName,
                AbpKafkaEventBusOptions.GroupId,
                AbpKafkaEventBusOptions.ConnectionName);

            Consumer.OnMessageReceived(ProcessEventAsync);

            SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
        }

        private async Task ProcessEventAsync(Message<string, byte[]> message)
        {
            var eventName = message.Key;
            var eventType = EventTypes.GetOrDefault(eventName);
            if (eventType == null)
            {
                return;
            }

            var eventData = Serializer.Deserialize(message.Value, eventType);

            await TriggerHandlersAsync(eventType, eventData);
        }

        public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);

            if (factory.IsInFactories(handlerFactories))
            {
                return NullDisposable.Instance;
            }

            handlerFactories.Add(factory);

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
                            factory is SingleInstanceHandlerFactory handlerFactory &&
                            handlerFactory.HandlerInstance == handler
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
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            var body = Serializer.Serialize(eventData);

            var producer = ProducerPool.Get(AbpKafkaEventBusOptions.ConnectionName);

            await producer.ProduceAsync(
                AbpKafkaEventBusOptions.TopicName,
                new Message<string, byte[]>
                {
                    Key = eventName, Value = body
                });
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
