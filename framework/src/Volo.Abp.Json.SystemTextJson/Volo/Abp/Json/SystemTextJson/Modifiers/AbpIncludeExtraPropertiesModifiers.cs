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
        if (typeof(IHasExtraProperties).IsAssignableFrom(jsonTypeInfo.Type))
        {
            var propertyJsonInfo = jsonTypeInfo.Properties
                .Where(x => x.AttributeProvider is MemberInfo)
                .FirstOrDefault(x =>
                    x.PropertyType == typeof(ExtraPropertyDictionary) &&
                    x.AttributeProvider!.As<MemberInfo>().Name == nameof(ExtensibleObject.ExtraProperties) &&
                    x.Set == null);

            if (propertyJsonInfo != null)
            {
                propertyJsonInfo.Set = (obj, value) =>
                {
                    ObjectHelper.TrySetProperty(obj.As<IHasExtraProperties>(), x => x.ExtraProperties, () => value);
                };
            }
        }
    }
}
