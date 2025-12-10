using System;
using System.Text.Json;

namespace Volo.Abp.Internal.Telemetry.Activity;

static internal class TelemetryJsonExtensions
{
    static internal string? GetStringOrNull(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property)
            ? property.GetString() ?? null
            : null;
    }

    static internal bool? GetBooleanOrNull(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var property) && bool.TryParse(property.GetString(), out var boolValue))
        {
            return boolValue;
        }

        return null;
    }

    static internal DateTimeOffset? GetDateTimeOffsetOrNull(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var date) && DateTimeOffset.TryParse(date.GetString(), out var dateTimeValue))
        {
            return dateTimeValue;
        }

        return null;
    }
    
    static internal Guid? GetGuidOrNull(JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var guidProperty) && Guid.TryParse(guidProperty.GetString(), out var guidValue))
        {
            return guidValue;
        }

        return null;
    }
}