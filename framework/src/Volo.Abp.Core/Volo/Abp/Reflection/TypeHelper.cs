using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.Reflection;

public static class TypeHelper
{
    private static readonly FrozenSet<Type> FloatingTypes = new HashSet<Type>
        {
            typeof(float),
            typeof(double),
            typeof(decimal)
        }.ToFrozenSet();

    private static readonly FrozenSet<Type> NonNullablePrimitiveTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(bool),
            typeof(float),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        }.ToFrozenSet();

    public static bool IsNonNullablePrimitiveType(Type type)
    {
        return NonNullablePrimitiveTypes.Contains(type);
    }

    public static bool IsFunc(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        var type = obj.GetType();
        if (!type.GetTypeInfo().IsGenericType)
        {
            return false;
        }

        return type.GetGenericTypeDefinition() == typeof(Func<>);
    }

    public static bool IsFunc<TReturn>(object? obj)
    {
        return obj != null && obj.GetType() == typeof(Func<TReturn>);
    }

    public static bool IsPrimitiveExtended(Type type, bool includeNullables = true, bool includeEnums = false)
    {
        if (IsPrimitiveExtendedInternal(type, includeEnums))
        {
            return true;
        }

        if (includeNullables && IsNullable(type) && type.GenericTypeArguments.Any())
        {
            return IsPrimitiveExtendedInternal(type.GenericTypeArguments[0], includeEnums);
        }

        return false;
    }

    public static TProperty? ChangeTypePrimitiveExtended<TProperty>(object? value)
    {
        if (value == null)
        {
            return default;
        }

        if (IsPrimitiveExtended(typeof(TProperty), includeEnums: true))
        {
            var conversionType = typeof(TProperty);
            if (IsNullable(conversionType))
            {
                conversionType = conversionType.GetFirstGenericArgumentIfNullable();
            }

            if (conversionType == typeof(Guid))
            {
                return (TProperty)TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(value.ToString()!)!;
            }

            if (conversionType.IsEnum)
            {
                return (TProperty)Enum.Parse(conversionType, value.ToString()!);
            }

            return (TProperty)ChangeType(value, conversionType);
        }

        throw new AbpException("ChangeTypePrimitiveExtended<TProperty> does not support non-primitive types. Use non-generic GetProperty method and handle type casting manually.");
    }

    /// <summary>
    /// Converts the value like <see cref="Convert.ChangeType(object, Type, IFormatProvider)"/> with the invariant culture.
    /// A float or double converted to decimal is rounded to the 7 or 15 significant digits the type holds, which is what
    /// earlier .NET versions returned for amounts (0.1d is 0.1m).
    /// </summary>
    public static object ChangeType(object value, Type conversionType)
    {
#if NET11_0_OR_GREATER
        // Since .NET 11 the conversion keeps the binary error: 0.1d became 0.1000000000000000055511151231m, so an amount
        // read back from JSON (where it is a double) would no longer equal the decimal that was saved.
        if (conversionType == typeof(decimal))
        {
            switch (value)
            {
                case double doubleValue:
                    return ToDecimal(doubleValue, doubleValue.ToString("G15", CultureInfo.InvariantCulture));
                case float floatValue:
                    return ToDecimal(floatValue, floatValue.ToString("G7", CultureInfo.InvariantCulture));
            }
        }
#endif

        return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
    }

#if NET11_0_OR_GREATER
    private static decimal ToDecimal(double value, string roundedValue)
    {
        // Convert.ChangeType throws OverflowException for NaN, infinity and values outside the decimal range; parsing the
        // rounded text would instead throw FormatException or round a value just past the range back into it.
        if (!(Math.Abs(value) < 7.922816251426434E+28))
        {
            throw new OverflowException("Value was either too large or too small for a Decimal.");
        }

        return decimal.Parse(roundedValue, NumberStyles.Float, CultureInfo.InvariantCulture);
    }
#endif

    public static bool IsNullable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static bool IsNullableEnum(Type type)
    {
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
               type.GenericTypeArguments.Length == 1 &&
               type.GenericTypeArguments[0].IsEnum;
    }

    public static Type GetFirstGenericArgumentIfNullable(this Type t)
    {
        if (t.GetGenericArguments().Length > 0 && t.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return t.GetGenericArguments().First();
        }

        return t;
    }

    public static bool IsEnumerable(Type type, out Type? itemType, bool includePrimitives = true)
    {
        if (!includePrimitives && IsPrimitiveExtended(type))
        {
            itemType = null;
            return false;
        }

        var enumerableTypes = ReflectionHelper.GetImplementedGenericTypes(type, typeof(IEnumerable<>));
        if (enumerableTypes.Count == 1)
        {
            itemType = enumerableTypes[0].GenericTypeArguments[0];
            return true;
        }

        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            itemType = typeof(object);
            return true;
        }

        itemType = null;
        return false;
    }

    public static bool IsDictionary(Type type, out Type? keyType, out Type? valueType)
    {
        var knownDictionaryInterfaces = new Type[]
        {
            typeof(IDictionary<,>),
            typeof(IReadOnlyDictionary<,>),
            typeof(IImmutableDictionary<,>)
        };

        foreach (var dictInterface in knownDictionaryInterfaces)
        {
            var dictionaryTypes = ReflectionHelper.GetImplementedGenericTypes(type, dictInterface);
            if (dictionaryTypes.Count == 1)
            {
                keyType = dictionaryTypes[0].GenericTypeArguments[0];
                valueType = dictionaryTypes[0].GenericTypeArguments[1];
                return true;
            }
        }

        if (typeof(IDictionary).IsAssignableFrom(type))
        {
            keyType = typeof(object);
            valueType = typeof(object);
            return true;
        }

        keyType = null;
        valueType = null;
        return false;
    }

    private static bool IsPrimitiveExtendedInternal(Type type, bool includeEnums)
    {
        if (type.IsPrimitive)
        {
            return true;
        }

        if (includeEnums && type.IsEnum)
        {
            return true;
        }

        return type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
#if NETCOREAPP
               type == typeof(DateOnly) ||
               type == typeof(TimeOnly) ||
#endif
               type == typeof(TimeSpan) ||
               type == typeof(Guid);
    }

    public static T? GetDefaultValue<T>()
    {
        return default;
    }

    public static object? GetDefaultValue(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    public static string GetFullNameHandlingNullableAndGenerics([NotNull] Type type)
    {
        Check.NotNull(type, nameof(type));

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return type.GenericTypeArguments[0].FullName + "?";
        }

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            var genericTypeFullName = genericType.FullName!;
            return $"{genericTypeFullName.Left(genericTypeFullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetFullNameHandlingNullableAndGenerics).JoinAsString(",")}>";
        }

        return type.FullName ?? type.Name;
    }

    public static string GetSimplifiedName([NotNull] Type type)
    {
        Check.NotNull(type, nameof(type));

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return GetSimplifiedName(type.GenericTypeArguments[0]) + "?";
        }

        if (type.IsGenericType)
        {
            var genericType = type.GetGenericTypeDefinition();
            var genericTypeFullName = genericType.FullName!;
            return $"{genericTypeFullName.Left(genericTypeFullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetSimplifiedName).JoinAsString(",")}>";
        }

        if (type == typeof(string))
        {
            return "string";
        }
        else if (type == typeof(int))
        {
            return "number";
        }
        else if (type == typeof(long))
        {
            return "number";
        }
        else if (type == typeof(bool))
        {
            return "boolean";
        }
        else if (type == typeof(char))
        {
            return "string";
        }
        else if (type == typeof(double))
        {
            return "number";
        }
        else if (type == typeof(float))
        {
            return "number";
        }
        else if (type == typeof(decimal))
        {
            return "number";
        }
        else if (type == typeof(DateTime))
        {
            return "string";
        }
        else if (type == typeof(DateTimeOffset))
        {
            return "string";
        }
        else if (type.FullName == "System.DateOnly")
        {
            return "string";
        }
        else if (type.FullName == "System.TimeOnly")
        {
            return "string";
        }
        else if (type == typeof(TimeSpan))
        {
            return "string";
        }
        else if (type == typeof(Guid))
        {
            return "string";
        }
        else if (type == typeof(byte))
        {
            return "number";
        }
        else if (type == typeof(sbyte))
        {
            return "number";
        }
        else if (type == typeof(short))
        {
            return "number";
        }
        else if (type == typeof(ushort))
        {
            return "number";
        }
        else if (type == typeof(uint))
        {
            return "number";
        }
        else if (type == typeof(ulong))
        {
            return "number";
        }
        else if (type == typeof(IntPtr))
        {
            return "number";
        }
        else if (type == typeof(UIntPtr))
        {
            return "number";
        }
        else if (type == typeof(object))
        {
            return "object";
        }
        else if (type.IsEnum)
        {
            return "enum";
        }

        return type.FullName ?? type.Name;
    }

    public static object? ConvertFromString<TTargetType>(string value)
    {
        return ConvertFromString(typeof(TTargetType), value);
    }

    public static object? ConvertFromString(Type targetType, string? value)
    {
        if (value == null)
        {
            return null;
        }

        var converter = TypeDescriptor.GetConverter(targetType);

        if (IsFloatingType(targetType))
        {
            using (CultureHelper.Use(CultureInfo.InvariantCulture))
            {
                return converter.ConvertFromString(value.Replace(',', '.'));
            }
        }

        return converter.ConvertFromString(value);
    }

    public static bool IsFloatingType(Type type, bool includeNullable = true)
    {
        if (FloatingTypes.Contains(type))
        {
            return true;
        }

        if (includeNullable &&
            IsNullable(type) &&
            FloatingTypes.Contains(type.GenericTypeArguments[0]))
        {
            return true;
        }

        return false;
    }

    public static object ConvertFrom<TTargetType>(object value)
    {
        return ConvertFrom(typeof(TTargetType), value);
    }

    public static object ConvertFrom(Type targetType, object value)
    {
        return TypeDescriptor
            .GetConverter(targetType)
            .ConvertFrom(value)!;
    }

    public static Type StripNullable(Type type)
    {
        return IsNullable(type)
            ? type.GenericTypeArguments[0]
            : type;
    }

    public static bool IsDefaultValue(object? obj)
    {
        if (obj == null)
        {
            return true;
        }

        return obj.Equals(GetDefaultValue(obj.GetType()));
    }
}
