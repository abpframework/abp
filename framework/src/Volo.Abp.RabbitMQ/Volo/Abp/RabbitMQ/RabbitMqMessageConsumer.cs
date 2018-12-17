using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Volo.Abp.RabbitMQ
{
    public class RabbitMqMessageConsumer : IRabbitMqMessageConsumer
    {
        protected IConnectionPool ConnectionPool { get; }
        protected ExchangeDeclareConfiguration Exchange { get; }
        protected QueueDeclareConfiguration Queue { get; }
        protected string ConnectionName { get; }
        protected IModel ConsumerChannel { get; set; }
        protected ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>> Callbacks { get; }

        public RabbitMqMessageConsumer(
            IConnectionPool connectionPool,
            ExchangeDeclareConfiguration exchange,
            QueueDeclareConfiguration queue,
            string connectionName = null)
        {
            ConnectionPool = connectionPool;
            Exchange = exchange;
            Queue = queue;
            ConnectionName = connectionName;
            Callbacks = new ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>>();
            ConsumerChannel = CreateConsumerChannel();
        }

        public void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback)
        {
            Callbacks.Add(callback);
        }

        private IModel CreateConsumerChannel()
        {
            var channel = ConnectionPool
                .Get(ConnectionName)
                .CreateModel();

            channel.ExchangeDeclare(
                exchange: Exchange.ExchangeName,
                type: Exchange.Type
            );

            channel.QueueDeclare(
                queue: Queue.QueueName,
                durable: Queue.Durable,
                exclusive: Queue.Exclusive,
                autoDelete: Queue.AutoDelete,
                arguments: Queue.Arguments
            );

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, basicDeliverEventArgs) =>
            {
                {
                    foreach (var callback in Callbacks)
                    {
                        await callback(channel, basicDeliverEventArgs);
                    }
                }
            };

            channel.BasicConsume(
                queue: Queue.QueueName,
                autoAck: false,
                consumer: consumer
            );

            //TODO: Instead of creating a new customer immediately without exception handling,
            //create a timer that constantly checks connection health and re-connect if needed.
            //As similar. Do not connect on constructor!

            channel.CallbackException += (sender, ea) =>
            {
                ConsumerChannel.Dispose();
                ConsumerChannel = CreateConsumerChannel();
            };

            return channel;
        }
    }
}
