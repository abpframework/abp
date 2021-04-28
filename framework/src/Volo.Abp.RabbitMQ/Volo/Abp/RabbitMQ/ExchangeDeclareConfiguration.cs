using System.Collections.Generic;

namespace Volo.Abp.RabbitMQ
{
    public class ExchangeDeclareConfiguration
    {
        public string ExchangeName { get; }

        public string DeadLetterExchangeName { get; set; }

        public string Type { get; }

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }

        public IDictionary<string, object> Arguments { get; }

        public ExchangeDeclareConfiguration(
            string exchangeName,
            string deadLetterExchangeName,
            string type,
            bool durable = false,
            bool autoDelete = false)
        {
            ExchangeName = exchangeName;
            DeadLetterExchangeName = deadLetterExchangeName;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = new Dictionary<string, object>();
        }
    }
}
