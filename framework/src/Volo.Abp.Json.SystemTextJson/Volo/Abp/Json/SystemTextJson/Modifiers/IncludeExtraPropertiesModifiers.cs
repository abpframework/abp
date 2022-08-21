using System;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Json.SystemTextJson.Modifiers;

public static class IncludeExtraPropertiesModifiers
{
    public static void Modify(JsonTypeInfo jsonTypeInfo)
    {
        var propertyJsonInfo = jsonTypeInfo.Properties.FirstOrDefault(x =>
            x.PropertyType == typeof(ExtraPropertyDictionary) &&
            x.Name.Equals(nameof(ExtensibleObject.ExtraProperties), StringComparison.OrdinalIgnoreCase) &&
            x.Set == null);

        if (propertyJsonInfo != null)
        {
            propertyJsonInfo.Set = (extraProperties, value) =>
            {
                ObjectHelper.TrySetProperty(extraProperties.As<ExtensibleObject>(), x => x.ExtraProperties, () => (ExtraPropertyDictionary)value);
            };
        }
    }
}
