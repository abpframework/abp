using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson.JsonConverters;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.EntityFrameworkCore.ValueConverters;

public class ExtraPropertiesValueConverter<TEntityType> : ValueConverter<ExtraPropertyDictionary, string>
{
    public ExtraPropertiesValueConverter()
        : base(
            d => SerializeObject(d),
            s => DeserializeObject(s))
    {

    }

    public readonly static JsonSerializerOptions SerializeOptions = new JsonSerializerOptions();

    private static string SerializeObject(ExtraPropertyDictionary extraProperties)
    {
        var copyDictionary = new Dictionary<string, object?>(extraProperties);

        var entityType = typeof(TEntityType);
        if (entityType != null)
        {
            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension != null)
            {
                foreach (var property in objectExtension.GetProperties())
                {
                    if (property.IsMappedToFieldForEfCore())
                    {
                        copyDictionary.Remove(property.Name);
                    }
                }
            }
        }

        return JsonSerializer.Serialize(copyDictionary, SerializeOptions);
    }

    public readonly static JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ObjectToInferredTypesConverter()
        }
    };

    private static ExtraPropertyDictionary DeserializeObject(string extraPropertiesAsJson)
    {
        if (extraPropertiesAsJson.IsNullOrEmpty() || extraPropertiesAsJson == "{}")
        {
            return new ExtraPropertyDictionary();
        }

        var dictionary = JsonSerializer.Deserialize<ExtraPropertyDictionary>(extraPropertiesAsJson, DeserializeOptions) ??
                            new ExtraPropertyDictionary();

        var entityType = typeof(TEntityType);
        if (entityType != null)
        {
            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension != null)
            {
                foreach (var property in objectExtension.GetProperties())
                {
                    dictionary[property.Name] = GetNormalizedValue(dictionary!, property);
                }
            }
        }

        return dictionary;
    }

    private static object? GetNormalizedValue(
        Dictionary<string, object> dictionary,
        ObjectExtensionPropertyInfo property)
    {
        var value = dictionary.GetOrDefault(property.Name);
        if (value == null)
        {
            return property.GetDefaultValue();
        }

        try
        {
            if (property.Type.IsEnum)
            {
                return Enum.Parse(property.Type, value.ToString()!, true);
            }

            //return Convert.ChangeType(value, property.Type);
            return value;
        }
        catch
        {
            return value;
        }
    }
}
