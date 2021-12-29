using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations;

public class AbpValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly ValidationAttributeAdapterProvider _defaultAdapter;

    public AbpValidationAttributeAdapterProvider(ValidationAttributeAdapterProvider defaultAdapter)
    {
        _defaultAdapter = defaultAdapter;
    }

    public virtual IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
    {
        var type = attribute.GetType();

        if (type == typeof(DynamicStringLengthAttribute))
        {
            return new DynamicStringLengthAttributeAdapter((DynamicStringLengthAttribute)attribute, stringLocalizer);
        }

        if (type == typeof(DynamicMaxLengthAttribute))
        {
            return new DynamicMaxLengthAttributeAdapter((DynamicMaxLengthAttribute)attribute, stringLocalizer);
        }

        if (type == typeof(DynamicRangeAttribute))
        {
            return new DynamicRangeAttributeAdapter((DynamicRangeAttribute)attribute, stringLocalizer);
        }

        return _defaultAdapter.GetAttributeAdapter(attribute, stringLocalizer);
    }
}
