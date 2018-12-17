using Volo.Abp.DependencyInjection;

namespace Volo.Abp.RabbitMQ
{
    public class RabbitMqMessageConsumerFactory : IRabbitMqMessageConsumerFactory, ISingletonDependency
    {
        protected IConnectionPool ConnectionPool { get; }

        public RabbitMqMessageConsumerFactory(IConnectionPool connectionPool)
        {
            ConnectionPool = connectionPool;
        }

        public IRabbitMqMessageConsumer Create(
            ExchangeDeclareConfiguration exchange,
            QueueDeclareConfiguration queue,
            string connectionName = null)
        {
            return new RabbitMqMessageConsumer(
                ConnectionPool,
                exchange,
                queue,
                connectionName
            );
        }
    }
}