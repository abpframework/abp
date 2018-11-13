using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.RabbitMQ;
using RabbitMQ.Client;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    /* Inspired from the implementation of "eShopOnContainers"
     * TODO: Implement Retry system
     * TODO: Should be improved
     */
    public class RabbitMqDistributedEventBus : IDistributedEventBus, ITransientDependency
    {
        protected RabbitMqDistributedEventBusOptions Options { get; }
        protected IChannelPool ChannelPool { get; }
        protected IRabbitMqSerializer Serializer { get; }

        public RabbitMqDistributedEventBus(
            IOptions<RabbitMqDistributedEventBusOptions> options,
            IChannelPool channelPool,
            IRabbitMqSerializer serializer)
        {
            ChannelPool = channelPool;
            Serializer = serializer;
            Options = options.Value;
        }

        public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAll<TEvent>() where TEvent : class
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeAll(Type eventType)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class
        {
            return PublishAsync(typeof(TEvent), eventData);
        }

        public Task PublishAsync(Type eventType, object eventData)
        {
            var eventName = eventType.FullName; //TODO: Get eventname from an attribute if available
            var body = Serializer.Serialize(eventData);

            using (var channelAccessor = ChannelPool.Acquire(Guid.NewGuid().ToString()))
            {
                //TODO: Other properties like durable?
                channelAccessor.Channel.ExchangeDeclare(Options.ExchangeName, "");
                
                var properties = channelAccessor.Channel.CreateBasicProperties();
                properties.DeliveryMode = 2; //persistent

                channelAccessor.Channel.BasicPublish(
                    exchange: Options.ExchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            }

            return Task.CompletedTask;
        }
    }
}