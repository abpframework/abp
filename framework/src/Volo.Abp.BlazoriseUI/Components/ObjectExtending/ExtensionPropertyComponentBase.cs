using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Blazorise;
using Blazorise.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public abstract class ExtensionPropertyComponentBase<TEntity, TResourceType> : OwningComponentBase
    where TEntity : IHasExtraProperties
{
    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; }

    [Inject]
    public IValidationMessageLocalizerAttributeFinder ValidationMessageLocalizerAttributeFinder { get; set; }

    [Parameter]
    public TEntity Entity { get; set; }

    [Parameter]
    public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

    [Parameter]
    public AbpBlazorMessageLocalizerHelper<TResourceType> LH { get; set; }

    protected virtual void Validate(ValidatorEventArgs e)
    {
        e.Status = ValidationStatus.Success;

        var result = ExtensibleObjectValidator.GetValidationErrors(Entity, PropertyInfo.Name, e.Value);
        if (!result.Any())
        {
            return;
        }

        if (LH == null)
        {
            e.MemberNames = result.First().MemberNames;
            e.Status = ValidationStatus.Error;
            e.ErrorText = result.First().ErrorMessage;
            return;
        }


        var formattedValidationAttributes = PropertyInfo.GetValidationAttributes();
        foreach (var validationAttribute in formattedValidationAttributes )
        {
            if (validationAttribute.ErrorMessageResourceName == null)
            {
                ValidationAttributeHelper.SetDefaultErrorMessage(validationAttribute);
            }
        }

        var formattedResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(Entity)
        {
            DisplayName = PropertyInfo.Name,
            MemberName = PropertyInfo.Name
        };

        Validator.TryValidateValue(e.Value, validationContext, formattedResults, formattedValidationAttributes);

        var errorMessage = result.First().ErrorMessage;
        var errorMessageString = ValidationAttributeHelper.RevertErrorMessagePlaceholders(formattedResults.First().ErrorMessage);
        var errorMessageArguments = ValidationMessageLocalizerAttributeFinder.FindAll(errorMessage, errorMessageString)
            ?.OrderBy(x => x.Index)
            ?.Select(x => x.Argument);

        e.MemberNames = result.First().MemberNames;
        e.Status = ValidationStatus.Error;
        e.ErrorText = LH.Localize(errorMessageString, errorMessageArguments);
    }
}
