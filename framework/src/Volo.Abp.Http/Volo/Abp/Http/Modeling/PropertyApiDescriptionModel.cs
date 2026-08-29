using System;
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
            Minimum = rangeAttribute != null ? Convert.ToString(rangeAttribute.Minimum, CultureInfo.InvariantCulture) : null,
            Maximum = rangeAttribute != null ? Convert.ToString(rangeAttribute.Maximum, CultureInfo.InvariantCulture) : null,
            MinimumIsExclusive = GetMinimumIsExclusive(rangeAttribute),
            MaximumIsExclusive = GetMaximumIsExclusive(rangeAttribute),
            MinLength = customAttributes.OfType<MinLengthAttribute>().FirstOrDefault()?.Length ?? customAttributes.OfType<StringLengthAttribute>().FirstOrDefault()?.MinimumLength,
            MaxLength = customAttributes.OfType<MaxLengthAttribute>().FirstOrDefault()?.Length ?? customAttributes.OfType<StringLengthAttribute>().FirstOrDefault()?.MaximumLength,
            Regex= customAttributes.OfType<RegularExpressionAttribute>().Select(x => x.Pattern).FirstOrDefault()
        };
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
