namespace Volo.Abp.RabbitMQ
{
    public class AbpRabbitMqOptions
    {
        public RabbitMqConnections Connections { get; }

        public QueueOptionsDictionary Queues { get; }

        public AbpRabbitMqOptions()
        {
            Connections = new RabbitMqConnections();
            Queues = new QueueOptionsDictionary();
        }
    }
}
