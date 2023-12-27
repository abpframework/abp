namespace Volo.Abp.EventBus.Dapr;

public class AbpDaprEventData
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }

    public string MessageId { get; set; }

    public string JsonData { get; set; }

    public string? CorrelationId { get; set; }

    public AbpDaprEventData(string pubSubName, string topic, string messageId, string jsonData, string? correlationId)
    {
        PubSubName = pubSubName;
        Topic = topic;
        MessageId = messageId;
        JsonData = jsonData;
        CorrelationId = correlationId;
    }
}
