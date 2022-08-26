using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Json.SystemTextJson.Modifiers;

public static class AbpIncludeExtraPropertiesModifiers
{
    public static void Modify(JsonTypeInfo jsonTypeInfo)
    {
        var propertyJsonInfo = jsonTypeInfo.Properties
            .Where(x => x.AttributeProvider != null && x.AttributeProvider is MemberInfo)
            .FirstOrDefault(x =>
                x.PropertyType == typeof(ExtraPropertyDictionary) &&
                x.AttributeProvider.As<MemberInfo>().Name == nameof(ExtensibleObject.ExtraProperties) &&
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
