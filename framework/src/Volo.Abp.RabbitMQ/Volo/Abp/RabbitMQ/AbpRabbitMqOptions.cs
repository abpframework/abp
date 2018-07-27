namespace Volo.Abp.RabbitMQ
{
    public class AbpRabbitMqOptions
    {
        public RabbitMqConnections ConnectionFactories { get; }

        public AbpRabbitMqOptions()
        {
            ConnectionFactories = new RabbitMqConnections();
        }
    }
}
