using System;
namespace Volo.Abp.EventBus;

/// <summary>
/// Wraps arbitrary event data with a string-based event name for anonymous (type-less) event handling.
/// Acts as both an envelope and event type for events that are identified by name rather than CLR type.
/// </summary>
[Serializable]
public class AnonymousEventData
{
    /// <summary>
    /// The string-based name that identifies the event.
    /// </summary>
    public string EventName { get; }

    /// <summary>
    /// The raw event data payload. Can be a CLR object, <see cref="System.Text.Json.JsonElement"/>, or any serializable object.
    /// </summary>
    internal object Data { get; }

    /// <summary>
    /// The raw JSON payload when the event is created from transport data.
    /// </summary>
    public string? JsonData { get; }

    /// <summary>
    /// Creates a new instance of <see cref="AnonymousEventData"/>.
    /// </summary>
    /// <param name="eventName">The string-based name that identifies the event</param>
    /// <param name="data">The raw event data payload</param>
    public AnonymousEventData(string eventName, object data)
    {
        EventName = eventName;
        Data = data;
    }

    /// <summary>
    /// Creates a new instance of <see cref="AnonymousEventData"/> from raw JSON.
    /// </summary>
    public static AnonymousEventData FromJson(string eventName, string jsonData)
    {
        return new AnonymousEventData(eventName, data: null!, jsonData);
    }

    private AnonymousEventData(string eventName, object data, string? jsonData)
    {
        EventName = eventName;
        Data = data;
        JsonData = jsonData;
    }
}
