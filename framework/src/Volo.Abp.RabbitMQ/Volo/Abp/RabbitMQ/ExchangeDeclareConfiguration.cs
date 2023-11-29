using System.Collections.Generic;

namespace Volo.Abp.RabbitMQ;

public class ExchangeDeclareConfiguration
{
    public string ExchangeName { get; }

    public string Type { get; }

    public bool Durable { get; set; }

    public bool AutoDelete { get; set; }

    public IDictionary<string, object> Arguments { get; }

    public ExchangeDeclareConfiguration(
        string exchangeName,
        string type,
        bool durable = false,
        bool autoDelete = false,
        IDictionary<string, object>? arguments = null)
    {
        ExchangeName = exchangeName;
        Type = type;
        Durable = durable;
        AutoDelete = autoDelete;
        Arguments = arguments?? new Dictionary<string, object>();
    }
}
