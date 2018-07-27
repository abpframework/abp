namespace Volo.Abp.RabbitMQ
{
    public class AbpRabbitMqOptions
    {
        public RabbitMqConnections ConnectionFactories { get; }

        public QueueOptionsDictionary Queues { get; }

        public AbpRabbitMqOptions()
        {
            ConnectionFactories = new RabbitMqConnections();
            Queues = new QueueOptionsDictionary();
        }
    }
}
