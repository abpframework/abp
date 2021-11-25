using System.Globalization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations;

public class DynamicMaxLengthAttributeAdapter : AttributeAdapterBase<DynamicMaxLengthAttribute>
{
    private readonly string _max;

    public DynamicMaxLengthAttributeAdapter(
        DynamicMaxLengthAttribute attribute,
        IStringLocalizer stringLocalizer)
        : base(attribute, stringLocalizer)
    {
        _max = Attribute.Length.ToString(CultureInfo.InvariantCulture);
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        Check.NotNull(validationContext, nameof(validationContext));

        return GetErrorMessage(
            validationContext.ModelMetadata,
            validationContext.ModelMetadata.GetDisplayName(),
            Attribute.Length
        );
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        Check.NotNull(context, nameof(context));

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-maxlength", GetErrorMessage(context));
        MergeAttribute(context.Attributes, "data-val-maxlength-max", _max);

        if (Attribute.Length != int.MaxValue)
        {
            MergeAttribute(context.Attributes, "data-val-maxlength-max", _max);
        }
    }
}
