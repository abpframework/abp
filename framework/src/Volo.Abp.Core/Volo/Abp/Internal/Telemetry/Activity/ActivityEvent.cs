using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Volo.Abp.Internal.Telemetry.Constants;

namespace Volo.Abp.Internal.Telemetry.Activity;

public class ActivityEvent : Dictionary<string, object?>
{
    public ActivityEvent()
    {
       this[ActivityPropertyNames.Id] = Guid.NewGuid();
       this[ActivityPropertyNames.Time] = DateTimeOffset.UtcNow;
    }

    public ActivityEvent(string activityName, string? details = null) : this()
    {
        Check.NotNullOrWhiteSpace(activityName, nameof(activityName));
        
        this[ActivityPropertyNames.ActivityName] = activityName;
        this[ActivityPropertyNames.ActivityDetails] = details;
    }

    public bool HasSolutionInfo()
    {
        return this.ContainsKey(ActivityPropertyNames.HasSolutionInfo);
    }

    public bool HasDeviceInfo()
    {
        return this.ContainsKey(ActivityPropertyNames.HasDeviceInfo);
    }

    public bool HasProjectInfo()
    {
        return this.ContainsKey(ActivityPropertyNames.HasProjectInfo);
    }

    public T Get<T>(string key)
    {
        return TryConvert<T>(key, out var value) ? value : default!;
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        return TryConvert(key, out value);
    }

    private bool TryConvert<T>(string key, out T result)
    {
        result = default!;
        if (!this.TryGetValue(key, out var value) || value is null)
        {
            return false;
        }

        try
        {
            if (value is T tValue)
            {
                result = tValue;
                return true;
            }

            if (value is JsonElement jsonElement)
            {
                value = ExtractFromJsonElement(jsonElement);
                if (value is null)
                {
                    return false;
                }
            }

            var targetType = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (underlyingType.IsEnum)
            {
                if (value is string str)
                {
                    result = (T)Enum.Parse(underlyingType, str, ignoreCase: true);
                }
                else if (value is int intValue)
                {
                    result = (T)Enum.ToObject(underlyingType, intValue);
                }

                return true;
            }


            if (underlyingType == typeof(Dictionary<string, object>[]))
            {
                result = (T)value;
                return true;
            }

            if (underlyingType == typeof(Guid))
            {
                result = (T)(object)Guid.Parse(value.ToString()!);
                return true;
            }

            if (underlyingType == typeof(DateTimeOffset))
            {
                result = (T)(object)DateTimeOffset.Parse(value.ToString()!);
                return true;
            }

            // Nullable types
            result = (T)Convert.ChangeType(value, underlyingType);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static object? ExtractFromJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.GetInt32(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            JsonValueKind.Array => element.EnumerateArray()
                .Select(item =>
                {
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        return item.EnumerateObject()
                            .ToDictionary(prop => prop.Name, prop => ExtractFromJsonElement(prop.Value));
                    }

                    return new Dictionary<string, object?> { { "value", ExtractFromJsonElement(item) } };
                })
                .ToArray(),

            JsonValueKind.Object => element.EnumerateObject()
                .ToDictionary(prop => prop.Name, prop => ExtractFromJsonElement(prop.Value)),
            _ => element.ToString()
        };
    }
}