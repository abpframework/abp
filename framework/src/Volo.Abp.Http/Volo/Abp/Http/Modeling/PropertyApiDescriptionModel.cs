using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Volo.Abp.Http.ProxyScripting.Configuration;

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

    public string? Regex { get; set; }

    public static PropertyApiDescriptionModel Create(PropertyInfo propertyInfo)
    {
        var customAttributes = propertyInfo.GetCustomAttributes(true);
        return new PropertyApiDescriptionModel
        {
            Name = propertyInfo.Name,
            JsonName = AbpApiProxyScriptingConfiguration.PropertyNameGenerator.Invoke(propertyInfo),
            Type = ApiTypeNameHelper.GetTypeName(propertyInfo.PropertyType),
            TypeSimple = ApiTypeNameHelper.GetSimpleTypeName(propertyInfo.PropertyType),
            IsRequired = customAttributes.OfType<RequiredAttribute>().Any() || propertyInfo.GetCustomAttributesData().Any(attr => attr.AttributeType.Name == "RequiredMemberAttribute"),
            Minimum = customAttributes.OfType<RangeAttribute>().Select(x => x.Minimum).FirstOrDefault()?.ToString(),
            Maximum = customAttributes.OfType<RangeAttribute>().Select(x => x.Maximum).FirstOrDefault()?.ToString(),
            MinLength = customAttributes.OfType<MinLengthAttribute>().FirstOrDefault()?.Length ?? customAttributes.OfType<StringLengthAttribute>().FirstOrDefault()?.MinimumLength,
            MaxLength = customAttributes.OfType<MaxLengthAttribute>().FirstOrDefault()?.Length ?? customAttributes.OfType<StringLengthAttribute>().FirstOrDefault()?.MaximumLength,
            Regex= customAttributes.OfType<RegularExpressionAttribute>().Select(x => x.Pattern).FirstOrDefault()
        };
    }
}
