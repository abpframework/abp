using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Reflection;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class PropertyApiDescriptionModel
{
    private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
    {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(IntPtr),
        typeof(UIntPtr)
    };

    public string Name { get; set; } = default!;

    public string? JsonName { get; set; }

    public string Type { get; set; } = default!;

    public string TypeSimple { get; set; } = default!;

    public bool IsRequired { get; set; }

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    public string? Minimum { get; set; }

    public string? Maximum { get; set; }

    public bool? MinimumIsExclusive { get; set; }

    public bool? MaximumIsExclusive { get; set; }

    public string? Regex { get; set; }

    public bool IsNullable { get; set; }

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public string? DisplayName { get; set; }

    public static PropertyApiDescriptionModel Create(PropertyInfo propertyInfo)
    {
        var customAttributes = propertyInfo.GetCustomAttributes(true);
        var rangeAttribute = customAttributes.OfType<RangeAttribute>().FirstOrDefault();
        return new PropertyApiDescriptionModel
        {
            Name = propertyInfo.Name,
            JsonName = AbpApiProxyScriptingConfiguration.PropertyNameGenerator.Invoke(propertyInfo),
            Type = ApiTypeNameHelper.GetTypeName(propertyInfo.PropertyType),
            TypeSimple = ApiTypeNameHelper.GetSimpleTypeName(propertyInfo.PropertyType),
            IsRequired = customAttributes.OfType<RequiredAttribute>().Any() || propertyInfo.GetCustomAttributesData().Any(attr => attr.AttributeType.Name == "RequiredMemberAttribute"),
            IsNullable = ReflectionHelper.IsNullable(propertyInfo),
            Minimum = GetRangeBound(rangeAttribute, rangeAttribute?.Minimum),
            Maximum = GetRangeBound(rangeAttribute, rangeAttribute?.Maximum),
            MinimumIsExclusive = GetMinimumIsExclusive(rangeAttribute),
            MaximumIsExclusive = GetMaximumIsExclusive(rangeAttribute),
            MinLength = customAttributes.OfType<MinLengthAttribute>().FirstOrDefault()?.Length ?? customAttributes.OfType<StringLengthAttribute>().FirstOrDefault()?.MinimumLength,
            MaxLength = customAttributes.OfType<MaxLengthAttribute>().FirstOrDefault()?.Length ?? customAttributes.OfType<StringLengthAttribute>().FirstOrDefault()?.MaximumLength,
            Regex= customAttributes.OfType<RegularExpressionAttribute>().Select(x => x.Pattern).FirstOrDefault()
        };
    }

    private static string? GetRangeBound(RangeAttribute? rangeAttribute, object? bound)
    {
        if (rangeAttribute == null || bound == null)
        {
            return null;
        }

        // The Range(Type, string, string) constructor keeps its limits as strings until the
        // first validation. Converting one the way the attribute converts it reports the value
        // the attribute validates against, which is not always the value that was written down.
        // The attribute reads its limits in the culture of the request unless it opts into the
        // invariant one, so only that opt-in makes the reported limit stable across requests.
        if (bound is string text)
        {
            if (!NumericTypes.Contains(rangeAttribute.OperandType))
            {
                return text;
            }

            var converted = ConvertRangeLimitOrNull(text, rangeAttribute);
            return converted != null
                ? Convert.ToString(converted, CultureInfo.InvariantCulture)
                : text;
        }

        return Convert.ToString(bound, CultureInfo.InvariantCulture);
    }

    private static object? ConvertRangeLimitOrNull(string text, RangeAttribute rangeAttribute)
    {
        try
        {
            return TypeDescriptor
                .GetConverter(rangeAttribute.OperandType)
                .ConvertFromString(null, GetRangeLimitCulture(rangeAttribute), text);
        }
        catch (Exception)
        {
            // A limit that does not convert is reported the way it was written. The attribute
            // throws on it during the first validation, and failing the whole api definition
            // over one declaration would hide every other type.
            return null;
        }
    }

    private static CultureInfo GetRangeLimitCulture(RangeAttribute rangeAttribute)
    {
#if NET8_0_OR_GREATER
        return rangeAttribute.ParseLimitsInInvariantCulture ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;
#else
        return CultureInfo.CurrentCulture;
#endif
    }

    private static bool? GetMinimumIsExclusive(RangeAttribute? rangeAttribute)
    {
        if (rangeAttribute == null)
        {
            return null;
        }

#if NET8_0_OR_GREATER
        return rangeAttribute.MinimumIsExclusive;
#else
        return false;
#endif
    }

    private static bool? GetMaximumIsExclusive(RangeAttribute? rangeAttribute)
    {
        if (rangeAttribute == null)
        {
            return null;
        }

#if NET8_0_OR_GREATER
        return rangeAttribute.MaximumIsExclusive;
#else
        return false;
#endif
    }
}
