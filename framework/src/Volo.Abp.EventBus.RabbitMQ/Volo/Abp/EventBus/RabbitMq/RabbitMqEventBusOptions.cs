namespace Volo.Abp.EventBus.RabbitMq
{
    public class RabbitMqEventBusOptions
    {
        public string ConnectionName { get; set; }

        public string ClientName { get; set; }

        public string ExchangeName { get; set; }
    }
}
