namespace Volo.Abp.RabbitMQ
{
    public interface IRabbitMqMessageConsumerFactory
    {
        IRabbitMqMessageConsumer Create(
            ExchangeDeclareConfiguration exchange,
            QueueDeclareConfiguration queue,
            string connectionName = null
        );
    }
}