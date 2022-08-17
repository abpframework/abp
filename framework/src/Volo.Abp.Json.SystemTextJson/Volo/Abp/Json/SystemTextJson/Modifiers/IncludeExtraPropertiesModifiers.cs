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
        var propertyJsonInfo = jsonTypeInfo.Properties.FirstOrDefault(x => x.PropertyType == typeof(ExtraPropertyDictionary) &&
                                                                           x.Name.Equals(nameof(ExtensibleObject.ExtraProperties), StringComparison.OrdinalIgnoreCase) &&
                                                                           x.Set == null);
        if (propertyJsonInfo != null)
        {
            var propertyInfo = jsonTypeInfo.Type.GetProperty(nameof(ExtensibleObject.ExtraProperties));
            if (propertyInfo != null)
            {
                var jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(typeof(ExtraPropertyDictionary), propertyJsonInfo.Name);
                jsonPropertyInfo.Get = propertyInfo.GetValue;
                jsonPropertyInfo.Set = propertyInfo.SetValue;
                jsonTypeInfo.Properties.Remove(propertyJsonInfo);
                jsonTypeInfo.Properties.Add(jsonPropertyInfo);
            }
        }
    }
}
