using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.AspNetCore.Mvc.DataAnnotations;

public class EnumDataTypeAttributeAdapter : AttributeAdapterBase<EnumDataTypeAttribute>
{
    public EnumDataTypeAttributeAdapter(
        EnumDataTypeAttribute attribute,
        IStringLocalizer? stringLocalizer)
        : base(attribute, stringLocalizer)
    {
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        Check.NotNull(context, nameof(context));

        //There is no built-in client side validation rule for enum values.
        //This adapter is used to localize the error message on the server side.
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        Check.NotNull(validationContext, nameof(validationContext));

        return GetErrorMessage(
            validationContext.ModelMetadata,
            validationContext.ModelMetadata.GetDisplayName()
        );
    }
}
