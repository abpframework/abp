namespace Volo.Abp.RabbitMQ
{
    public class AbpRabbitMqOptions
    {
        public RabbitMqConnections Connections { get; }

        public QueueDictionary Queues { get; }

        public AbpRabbitMqOptions()
        {
            Connections = new RabbitMqConnections();
            Queues = new QueueDictionary();
        }
    }
}
