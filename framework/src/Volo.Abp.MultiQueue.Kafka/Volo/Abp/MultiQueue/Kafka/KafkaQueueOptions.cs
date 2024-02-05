using Confluent.Kafka;
using Volo.Abp.MultiQueue.Options;

namespace Volo.Abp.MultiQueue.Kafka;

[QueueOptionsType("Kafka")]
public class KafkaQueueOptions : QueueOptions, IQueueOptions
{
    public virtual string Address { get; set; }

    public virtual string GroupId { get; set; }

    public virtual string UserName { get; set; }

    public virtual string Password { get; set; }

    public virtual int? MessageMaxBytes { get; set; }

    public virtual SecurityProtocol? SecurityProtocol { get; set; }

    public virtual SaslMechanism? SaslMechanism { get; set; }
}
