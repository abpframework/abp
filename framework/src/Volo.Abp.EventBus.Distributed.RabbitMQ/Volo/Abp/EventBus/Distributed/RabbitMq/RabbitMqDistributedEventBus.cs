using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.RabbitMQ;
using RabbitMQ.Client;
using Volo.Abp.EventBus.Local;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    /* TODO: Implement Retry system
     * TODO: Should be improved
     */
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDistributedEventBus), typeof(RabbitMqDistributedEventBus))]
    public class RabbitMqDistributedEventBus : EventBusBase, IDistributedEventBus, ITransientDependency
    {
        protected RabbitMqDistributedEventBusOptions Options { get; }
        protected IConnectionPool ConnectionPool { get; }
        protected IRabbitMqSerializer Serializer { get; }

        public RabbitMqDistributedEventBus(
            IOptions<RabbitMqDistributedEventBusOptions> options,
            IConnectionPool connectionPool,
            IRabbitMqSerializer serializer)
        {
            ConnectionPool = connectionPool;
            Serializer = serializer;
            Options = options.Value;
        }
        
        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeAll(Type eventType)
        {
            throw new NotImplementedException();
        }

        public override Task PublishAsync(Type eventType, object eventData)
        {
            var eventName = EventNameAttribute.GetName(eventType);
            var body = Serializer.Serialize(eventData);

            using (var channel = ConnectionPool.Get().CreateModel())
            {
                //TODO: Other properties like durable?
                channel.ExchangeDeclare(Options.ExchangeName, "");
                
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; //persistent

                channel.BasicPublish(
                   exchange: Options.ExchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }

            return Task.CompletedTask;
        }

        protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            throw new NotImplementedException();
        }
    }
}