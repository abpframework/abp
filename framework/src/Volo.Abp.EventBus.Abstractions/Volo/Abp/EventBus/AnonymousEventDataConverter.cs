using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Volo.Abp.EventBus;

public static class AnonymousEventDataConverter
{
    public static T ConvertToTypedObject<T>(AnonymousEventData eventData)
    {
        if (eventData.Data is T typedData)
        {
            return typedData;
        }

        return ParseJsonElement(eventData).Deserialize<T>()
               ?? throw new InvalidOperationException($"Failed to deserialize AnonymousEventData to {typeof(T).FullName}.");
    }

    public static object ConvertToTypedObject(AnonymousEventData eventData, Type type)
    {
        if (type.IsInstanceOfType(eventData.Data))
        {
            return eventData.Data;
        }

        return ParseJsonElement(eventData).Deserialize(type)
               ?? throw new InvalidOperationException($"Failed to deserialize AnonymousEventData to {type.FullName}.");
    }

    public static object ConvertToLooseObject(AnonymousEventData eventData)
    {
        return ConvertElement(ParseJsonElement(eventData));
    }

    public static string GetJsonData(AnonymousEventData eventData)
    {
        return eventData.JsonData ?? ParseJsonElement(eventData).GetRawText();
    }

    private static JsonElement ParseJsonElement(AnonymousEventData eventData)
    {
        if (eventData.Data is JsonElement existingElement)
        {
            return existingElement;
        }

        if (eventData.JsonData != null)
        {
            return JsonDocument.Parse(eventData.JsonData).RootElement.Clone();
        }

        return JsonSerializer.SerializeToElement(eventData.Data);
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
