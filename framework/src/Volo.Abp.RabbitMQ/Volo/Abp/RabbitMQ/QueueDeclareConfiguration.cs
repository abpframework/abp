using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using RabbitMQ.Client;

namespace Volo.Abp.RabbitMQ;

public class QueueDeclareConfiguration
{
    [NotNull] public string QueueName { get; }

    public bool Durable { get; set; }

    public bool Exclusive { get; set; }

    public bool AutoDelete { get; set; }

    public ushort? PrefetchCount { get; set; }

    public IDictionary<string, object?> Arguments { get; }

    public QueueDeclareConfiguration(
        [NotNull] string queueName,
        bool durable = true,
        bool exclusive = false,
        bool autoDelete = false,
        ushort? prefetchCount = null,
        IDictionary<string, object?>? arguments = null)
    {
        QueueName = queueName;
        Durable = durable;
        Exclusive = exclusive;
        AutoDelete = autoDelete;
        Arguments = arguments?? new Dictionary<string, object?>();
        PrefetchCount = prefetchCount;
    }

    public virtual async Task<QueueDeclareOk> DeclareAsync(IChannel channel)
    {
        return await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: Durable,
            exclusive: Exclusive,
            autoDelete: AutoDelete,
            arguments: Arguments
        );
    }
}
