using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.RabbitMq
{
    /* TODO: How to handle unsubscribe to unbind on RabbitMq (may not be possible for)
     * TODO: Implement Retry system
     * TODO: Should be improved
     */
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDistributedEventBus), typeof(RabbitMqDistributedEventBus))]
    public class RabbitMqDistributedEventBus : EventBusBase, IDistributedEventBus, ISingletonDependency
    {
        protected AbpRabbitMqEventBusOptions AbpRabbitMqEventBusOptions { get; }
        protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }
        protected AbpEventBusOptions AbpEventBusOptions { get; }
        protected IConnectionPool ConnectionPool { get; }
        protected IRabbitMqSerializer Serializer { get; }

        //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected IRabbitMqMessageConsumerFactory MessageConsumerFactory { get; }
        protected IRabbitMqMessageConsumer Consumer { get; private set; }

        public RabbitMqDistributedEventBus(
            IOptions<AbpRabbitMqEventBusOptions> options,
            IConnectionPool connectionPool,
            IRabbitMqSerializer serializer,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions,
            IRabbitMqMessageConsumerFactory messageConsumerFactory,
            ICurrentTenant currentTenant,
            IEventErrorHandler errorHandler,
            IOptions<AbpEventBusOptions> abpEventBusOptions)
            : base(serviceScopeFactory, currentTenant, errorHandler)
        {
            ConnectionPool = connectionPool;
            Serializer = serializer;
            MessageConsumerFactory = messageConsumerFactory;
            AbpEventBusOptions = abpEventBusOptions.Value;
            AbpDistributedEventBusOptions = distributedEventBusOptions.Value;
            AbpRabbitMqEventBusOptions = options.Value;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
        }

        public void Initialize()
        {
            const string suffix = "_dead_letter";

            Consumer = MessageConsumerFactory.Create(
                new ExchangeDeclareConfiguration(
                    AbpRabbitMqEventBusOptions.ExchangeName,
                    type: "direct",
                    durable: true,
                    deadLetterExchangeName: AbpRabbitMqEventBusOptions.ExchangeName + suffix
                ),
                new QueueDeclareConfiguration(
                    AbpRabbitMqEventBusOptions.ClientName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    AbpEventBusOptions.DeadLetterName ?? AbpRabbitMqEventBusOptions.ClientName + suffix
                ),
                AbpRabbitMqEventBusOptions.ConnectionName
            );

            Consumer.OnMessageReceived(ProcessEventAsync);

            SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
        }

        private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
        {
            var eventName = ea.RoutingKey;
            var eventType = EventTypes.GetOrDefault(eventName);
            if (eventType == null)
            {
                return;
            }

            var eventData = Serializer.Deserialize(ea.Body.ToArray(), eventType);

            await TriggerHandlersAsync(eventType, eventData, errorContext =>
            {
                var retryAttempt = 0;
                if (ea.BasicProperties.Headers != null &&
                    ea.BasicProperties.Headers.ContainsKey(EventErrorHandlerBase.RetryAttemptKey))
                {
                    retryAttempt = (int)ea.BasicProperties.Headers[EventErrorHandlerBase.RetryAttemptKey];
                }

                errorContext.EventData = Serializer.Deserialize(ea.Body.ToArray(), eventType);
                errorContext.SetProperty(EventErrorHandlerBase.HeadersKey, ea.BasicProperties);
                errorContext.SetProperty(EventErrorHandlerBase.RetryAttemptKey, retryAttempt);
            });
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

            if (handlerFactories.Count == 1) //TODO: Multi-threading!
            {
                Consumer.BindAsync(EventNameAttribute.GetNameOrDefault(eventType));
            }

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
            await PublishAsync(eventType, eventData, null);
        }

        public Task PublishAsync(Type eventType, object eventData, IBasicProperties properties, Dictionary<string, object> headersArguments = null)
        {

            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            var body = Serializer.Serialize(eventData);

            using (var channel = ConnectionPool.Get(AbpRabbitMqEventBusOptions.ConnectionName).CreateModel())
            {
                channel.ExchangeDeclare(
                    AbpRabbitMqEventBusOptions.ExchangeName,
                    "direct",
                    durable: true
                );

                if (properties == null)
                {
                    properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;
                    properties.MessageId = Guid.NewGuid().ToString("N");
                }

                SetEventMessageHeaders(properties, headersArguments);

                channel.BasicPublish(
                    exchange: AbpRabbitMqEventBusOptions.ExchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }

            return Task.CompletedTask;
        }

        private void SetEventMessageHeaders(IBasicProperties properties, Dictionary<string, object> headersArguments)
        {
            if (headersArguments == null)
            {
                return;
            }

            properties.Headers ??= new Dictionary<string, object>();

            foreach (var header in headersArguments)
            {
                properties.Headers[header.Key] = header.Value;
            }
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

            foreach (var handlerFactory in
                HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
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

            //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }
    }
}
