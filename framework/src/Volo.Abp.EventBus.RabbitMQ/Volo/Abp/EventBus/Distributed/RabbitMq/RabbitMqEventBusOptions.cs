namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    public class RabbitMqEventBusOptions
    {
        public string ConnectionName { get; set; }

        public string ClientName { get; set; }

        public string ExchangeName { get; set; }
    }
}
