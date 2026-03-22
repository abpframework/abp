namespace Volo.Abp.EventBus;

/// <summary>
/// Wraps arbitrary event data with a string-based event name for dynamic (type-less) event handling.
/// </summary>
public class DynamicEventData
{
    public string EventName { get; }

    public object Data { get; }

    public DynamicEventData(string eventName, object data)
    {
        EventName = Check.NotNullOrWhiteSpace(eventName, nameof(eventName));
        Data = Check.NotNull(data, nameof(data));
    }
}
