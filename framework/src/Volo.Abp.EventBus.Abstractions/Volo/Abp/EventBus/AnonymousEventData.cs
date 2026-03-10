using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
    /// The raw event data payload. Can be a CLR object, <see cref="JsonElement"/>, or any serializable object.
    /// </summary>
    public object Data { get; }

    private JsonElement? _cachedJsonElement;

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
    /// Converts the <see cref="Data"/> to a loosely-typed object graph
    /// (dictionaries for objects, lists for arrays, primitives for values).
    /// </summary>
    /// <returns>A CLR object representation of the event data</returns>
    public object ConvertToTypedObject()
    {
        return ConvertElement(GetJsonElement());
    }

    /// <summary>
    /// Converts the <see cref="Data"/> to a strongly-typed <typeparamref name="T"/> object.
    /// Returns the data directly if it is already of type <typeparamref name="T"/>,
    /// otherwise deserializes from JSON.
    /// </summary>
    /// <typeparam name="T">Target type to convert to</typeparam>
    /// <returns>The deserialized object of type <typeparamref name="T"/></returns>
    /// <exception cref="InvalidOperationException">Thrown when deserialization fails</exception>
    public T ConvertToTypedObject<T>()
    {
        if (Data is T typedData)
        {
            return typedData;
        }

        return GetJsonElement().Deserialize<T>()
            ?? throw new InvalidOperationException($"Failed to deserialize AnonymousEventData to {typeof(T).FullName}.");
    }

    /// <summary>
    /// Converts the <see cref="Data"/> to the specified <paramref name="type"/>.
    /// Returns the data directly if it is already an instance of the target type,
    /// otherwise deserializes from JSON.
    /// </summary>
    /// <param name="type">Target type to convert to</param>
    /// <returns>The deserialized object</returns>
    /// <exception cref="InvalidOperationException">Thrown when deserialization fails</exception>
    public object ConvertToTypedObject(Type type)
    {
        if (type.IsInstanceOfType(Data))
        {
            return Data;
        }

        return GetJsonElement().Deserialize(type)
            ?? throw new InvalidOperationException($"Failed to deserialize AnonymousEventData to {type.FullName}.");
    }

    private JsonElement GetJsonElement()
    {
        if (_cachedJsonElement.HasValue)
        {
            return _cachedJsonElement.Value;
        }

        if (Data is JsonElement existingElement)
        {
            _cachedJsonElement = existingElement;
            return existingElement;
        }

        _cachedJsonElement = JsonSerializer.SerializeToElement(Data);
        return _cachedJsonElement.Value;
    }

    private static object ConvertElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                var obj = new Dictionary<string, object?>();
                foreach (var property in element.EnumerateObject())
                {
                    obj[property.Name] = property.Value.ValueKind == JsonValueKind.Null
                        ? null
                        : ConvertElement(property.Value);
                }
                return obj;
            }
            case JsonValueKind.Array:
                return element.EnumerateArray()
                    .Select(item => item.ValueKind == JsonValueKind.Null ? null : (object?)ConvertElement(item))
                    .ToList();
            case JsonValueKind.String:
                return element.GetString()!;
            case JsonValueKind.Number when element.TryGetInt64(out var longValue):
                return longValue;
            case JsonValueKind.Number when element.TryGetDecimal(out var decimalValue):
                return decimalValue;
            case JsonValueKind.Number when element.TryGetDouble(out var doubleValue):
                return doubleValue;
            case JsonValueKind.True:
                return true;
            case JsonValueKind.False:
                return false;
            case JsonValueKind.Null:
            case JsonValueKind.Undefined:
            default:
                return null!;
        }
    }
}
