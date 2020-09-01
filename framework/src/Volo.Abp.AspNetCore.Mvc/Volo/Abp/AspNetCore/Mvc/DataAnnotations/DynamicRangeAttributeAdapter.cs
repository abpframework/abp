using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations
{
    public class DynamicRangeAttributeAdapter : AttributeAdapterBase<DynamicRangeAttribute>
    {
        private readonly string _max;
        private readonly string _min;

        public DynamicRangeAttributeAdapter(
            DynamicRangeAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            _min = Attribute.Minimum.ToString()?.ToString(CultureInfo.InvariantCulture);
            _max = Attribute.Maximum.ToString()?.ToString(CultureInfo.InvariantCulture);
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            Check.NotNull(context, nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-length", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-length-min", _min);
            MergeAttribute(context.Attributes, "data-val-length-max", _max);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            Check.NotNull(validationContext, nameof(validationContext));

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Minimum,
                Attribute.Maximum
            );
        }
    }
}
