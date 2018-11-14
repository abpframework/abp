using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.Collections;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    /* TODO: How to handle unsubscribe to unbind on RabbitMq (may not be possible for)
     * TODO: Implement Retry system
     * TODO: Should be improved
     */
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDistributedEventBus), typeof(RabbitMqDistributedEventBus))]
    public class RabbitMqDistributedEventBus : EventBusBase, IDistributedEventBus, ISingletonDependency
    {
        protected RabbitMqDistributedEventBusOptions RabbitMqDistributedEventBusOptions { get; }
        protected DistributedEventBusOptions DistributedEventBusOptions { get; }
        protected IConnectionPool ConnectionPool { get; }
        protected IRabbitMqSerializer Serializer { get; }
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; } //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        protected IModel ConsumerChannel;
        protected IServiceProvider ServiceProvider { get; }

        public RabbitMqDistributedEventBus(
            IOptions<RabbitMqDistributedEventBusOptions> options,
            IConnectionPool connectionPool,
            IRabbitMqSerializer serializer,
            IServiceProvider serviceProvider, 
            DistributedEventBusOptions distributedEventBusOptions)
        {
            ConnectionPool = connectionPool;
            Serializer = serializer;
            ServiceProvider = serviceProvider;
            DistributedEventBusOptions = distributedEventBusOptions;
            RabbitMqDistributedEventBusOptions = options.Value;
            
            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();

            ConsumerChannel = CreateConsumerChannel();
            Subscribe(DistributedEventBusOptions.Handlers);
        }

        public virtual void Subscribe(ITypeList<IEventHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                    {
                        continue;
                    }

                    var genericArgs = @interface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceProvider, handler));
                    }
                }
            }
        }

        private IModel CreateConsumerChannel()
        {
            //TODO: Support multiple connection (and consumer)?
            var channel = ConnectionPool.Get().CreateModel();
            channel.ExchangeDeclare(
                exchange: RabbitMqDistributedEventBusOptions.ExchangeName,
                type: "direct"
            );

            channel.QueueDeclare(
                queue: RabbitMqDistributedEventBusOptions.ClientName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) => { await ProcessEventAsync(channel, ea); };

            channel.BasicConsume(
                queue: RabbitMqDistributedEventBusOptions.ClientName,
                autoAck: false,
                consumer: consumer
            );

            channel.CallbackException += (sender, ea) =>
            {
                ConsumerChannel.Dispose();
                ConsumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
        {
            var eventName = ea.RoutingKey;
            var eventType = EventTypes.GetOrDefault(eventName);
            if (eventType == null)
            {
                return;
            }

            var eventData = Serializer.Deserialize(ea.Body, eventType);

            await TriggerHandlersAsync(eventType, eventData);

            channel.BasicAck(ea.DeliveryTag, multiple: false);
        }

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);
            
            handlerFactories.Add(factory);

            if (handlerFactories.Count == 1) //TODO: Multi-threading!
            {
                var eventName = EventNameAttribute.GetName(eventType);

                using (var channel = ConnectionPool.Get().CreateModel()) //TODO: Connection name per event!
                {
                    channel.QueueBind(
                        queue: RabbitMqDistributedEventBusOptions.ClientName,
                        exchange: RabbitMqDistributedEventBusOptions.ExchangeName,
                        routingKey: eventName
                    );
                }
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

        public override Task PublishAsync(Type eventType, object eventData)
        {
            var eventName = EventNameAttribute.GetName(eventType);
            var body = Serializer.Serialize(eventData);

            using (var channel = ConnectionPool.Get().CreateModel()) //TODO: Connection name per event!
            {
                //TODO: Other properties like durable?
                channel.ExchangeDeclare(RabbitMqDistributedEventBusOptions.ExchangeName, "");
                
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; //persistent

                channel.BasicPublish(
                   exchange: RabbitMqDistributedEventBusOptions.ExchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }

            return Task.CompletedTask;
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetName(type);
                    EventTypes[eventName] = type;
                    return new List<IEventHandlerFactory>();
                }
            );
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