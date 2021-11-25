namespace Volo.Abp.EventBus.Kafka;

public class AbpKafkaEventBusOptions
{

    public string ConnectionName { get; set; }

    public string TopicName { get; set; }

    public string GroupId { get; set; }
}
