namespace Volo.Abp.RabbitMQ;

public interface IRabbitMqMessageConsumerFactory
{
    /// <summary>
    /// Creates a new <see cref="IRabbitMqMessageConsumer"/>.
    /// Avoid to create too many consumers since they are
    /// not disposed until end of the application.
    /// </summary>
    /// <param name="exchange"></param>
    /// <param name="queue"></param>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    IRabbitMqMessageConsumer Create(
        ExchangeDeclareConfiguration exchange,
        QueueDeclareConfiguration queue,
        string connectionName = null
    );
}
