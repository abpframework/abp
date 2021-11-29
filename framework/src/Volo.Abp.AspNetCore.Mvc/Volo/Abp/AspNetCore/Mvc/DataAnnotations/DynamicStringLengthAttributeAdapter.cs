using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations
{
    public class DynamicStringLengthAttributeAdapter : AttributeAdapterBase<DynamicStringLengthAttribute>
    {
        private readonly string _max;
        private readonly string _min;

        public DynamicStringLengthAttributeAdapter(
            DynamicStringLengthAttribute attribute,
            IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            _max = Attribute.MaximumLength.ToString(CultureInfo.InvariantCulture);
            _min = Attribute.MinimumLength.ToString(CultureInfo.InvariantCulture);
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            Check.NotNull(context, nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-length", GetErrorMessage(context));

            if (Attribute.MaximumLength != int.MaxValue)
            {
                MergeAttribute(context.Attributes, "data-val-length-max", _max);
            }

            if (Attribute.MinimumLength != 0)
            {
                MergeAttribute(context.Attributes, "data-val-length-min", _min);
            }
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            Check.NotNull(validationContext, nameof(validationContext));

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.MaximumLength,
                Attribute.MinimumLength
            );
        }
    }
}