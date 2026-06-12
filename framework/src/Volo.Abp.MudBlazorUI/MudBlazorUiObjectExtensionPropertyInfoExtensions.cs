using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.MudBlazorUI.Components.ObjectExtending;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Reflection;

namespace Volo.Abp.MudBlazorUI;

public static class MudBlazorUiObjectExtensionPropertyInfoExtensions
{
    private static readonly Type[] DateTimeTypes =
    {
        typeof(DateTime),
        typeof(DateTime?),
        typeof(DateTimeOffset),
        typeof(DateTimeOffset?)
    };

    internal static readonly FrozenSet<Type> NumberTypes = new HashSet<Type>
    {
        typeof(int),
        typeof(long),
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(uint),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(int?),
        typeof(long?),
        typeof(byte?),
        typeof(sbyte?),
        typeof(short?),
        typeof(ushort?),
        typeof(uint?),
        typeof(ulong?),
        typeof(float?),
        typeof(double?),
        typeof(decimal?)
    }.ToFrozenSet();

    private static readonly FrozenSet<Type> TextEditSupportedAttributeTypes = new HashSet<Type>
    {
        typeof(EmailAddressAttribute),
        typeof(UrlAttribute),
        typeof(PhoneAttribute)
    }.ToFrozenSet();

    public static bool IsDate(this IBasicObjectExtensionPropertyInfo property)
    {
        return DateTimeTypes.Contains(property.Type) &&
               property.GetDataTypeOrNull() == DataType.Date;
    }

    public static bool IsDateTime(this IBasicObjectExtensionPropertyInfo property)
    {
        return DateTimeTypes.Contains(property.Type) &&
               !property.IsDate();
    }

    public static DataType? GetDataTypeOrNull(this IBasicObjectExtensionPropertyInfo property)
    {
        return property
            .Attributes
            .OfType<DataTypeAttribute>()
            .FirstOrDefault()?.DataType;
    }

    public static string? GetDateEditInputFormatOrNull(this IBasicObjectExtensionPropertyInfo property)
    {
        var dataFormatString = property.GetDataFormatStringOrNull();
        if (dataFormatString != null)
        {
            return dataFormatString;
        }

        if (property.IsDate())
        {
            return "{0:yyyy-MM-dd}";
        }

        if (property.IsDateTime())
        {
            return "{0:yyyy-MM-ddTHH:mm}";
        }

        return null;
    }

    public static string? GetDataFormatStringOrNull(this IBasicObjectExtensionPropertyInfo property)
    {
        return property
            .Attributes
            .OfType<DisplayFormatAttribute>()
            .FirstOrDefault()?.DataFormatString;
    }

    public static string? GetTextInputValueOrNull(this IBasicObjectExtensionPropertyInfo property, object? value)
    {
        if (value == null)
        {
            return null;
        }

        if (TypeHelper.IsFloatingType(property.Type))
        {
            return value.ToString()?.Replace(',', '.');
        }

        return value.ToString();
    }

    public static T? GetInputValueOrDefault<T>(this IBasicObjectExtensionPropertyInfo property, object? value)
    {
        if (value == null)
        {
            return default;
        }

        return (T)value;
    }

    public static Type GetInputType(this ObjectExtensionPropertyInfo propertyInfo)
    {
        foreach (var attribute in propertyInfo.Attributes)
        {
            var inputTypeByAttribute = GetInputTypeFromAttributeOrNull(attribute, propertyInfo.Type);
            if (inputTypeByAttribute != null)
            {
                return inputTypeByAttribute;
            }
        }
        return GetInputTypeFromTypeOrNull(propertyInfo.Type)
               ?? typeof(MudTextExtensionProperty<,>); //default
    }

    public static bool IsEnum(this ObjectExtensionPropertyInfo propertyInfo)
    {
        return propertyInfo.Type.IsEnum || TypeHelper.IsNullableEnum(propertyInfo.Type);
    }

    private static Type? GetInputTypeFromAttributeOrNull(Attribute attribute, Type propertyType)
    {
        var hasTextEditSupport = TextEditSupportedAttributeTypes.Any(t => t == attribute.GetType());

        if (hasTextEditSupport)
        {
            return typeof(MudTextExtensionProperty<,>);
        }

        if (attribute is DataTypeAttribute dataTypeAttribute)
        {
            switch (dataTypeAttribute.DataType)
            {
                case DataType.Password:
                case DataType.EmailAddress:
                case DataType.Url:
                case DataType.PhoneNumber:
                    return typeof(MudTextExtensionProperty<,>);
                case DataType.Date:
                case DataType.DateTime:
                    return propertyType == typeof(DateTimeOffset) || propertyType == typeof(DateTimeOffset?)
                        ? typeof(MudDateTimeOffsetExtensionProperty<,>)
                        : typeof(MudDateTimeExtensionProperty<,>);
                case DataType.Time:
                    return typeof(MudTimeExtensionProperty<,>);
                case DataType.MultilineText:
                    return typeof(MudTextAreaExtensionProperty<,>);
            }
        }

        return null;
    }

    private static Type? GetInputTypeFromTypeOrNull(Type type)
    {
        if (type == typeof(bool))
        {
            return typeof(MudCheckExtensionProperty<,>);
        }

        if (type == typeof(DateTime) || type == typeof(DateTime?))
        {
            return typeof(MudDateTimeExtensionProperty<,>);
        }

        if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
        {
            return typeof(MudDateTimeOffsetExtensionProperty<,>);
        }

        if (NumberTypes.Contains(type))
        {
            return typeof(MudTextExtensionProperty<,>);
        }

        return null;
    }
}
