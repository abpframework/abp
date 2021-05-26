using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations
{
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
                return new DynamicStringLengthAttributeAdapter((DynamicStringLengthAttribute) attribute, stringLocalizer);
            }

            if (type == typeof(DynamicMaxLengthAttribute))
            {
                return new DynamicMaxLengthAttributeAdapter((DynamicMaxLengthAttribute) attribute, stringLocalizer);
            }

            if (type == typeof(DynamicRangeAttribute))
            {
                return new DynamicRangeAttributeAdapter((DynamicRangeAttribute) attribute, stringLocalizer);
            }

            //DataAnnotationsClientModelValidatorProvider will add a default '[Required]' validator for generating HTML if necessary.
            if (type == typeof(RequiredAttribute) && attribute.ErrorMessage == null)
            {
                ValidationAttributeHelper.SetDefaultErrorMessage(attribute);
            }

            return _defaultAdapter.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
