using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Reflection;

namespace Volo.Abp.ObjectExtending;

internal static class ExtensionPropertyHelper
{
    public static IEnumerable<Attribute> GetDefaultAttributes(Type type)
    {
        if (TypeHelper.IsNonNullablePrimitiveType(type) || type.IsEnum)
        {
            yield return new RequiredAttribute();
        }

        if (type.IsEnum)
        {
            yield return new EnumDataTypeAttribute(type);
        }
    }

    public static object GetDefaultValue(
        Type propertyType,
        Func<object> defaultValueFactory,
        object defaultValue)
    {
        if (defaultValueFactory != null)
        {
            return defaultValueFactory();
        }

        return defaultValue ??
               TypeHelper.GetDefaultValue(propertyType);
    }
}
