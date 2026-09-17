using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.MudBlazorUI.Validation;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public abstract class MudExtensionPropertyComponentBase<TEntity, TResourceType> : OwningComponentBase
    where TEntity : IHasExtraProperties
{
    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; } = default!;

    [Inject]
    public IAbpEnumLocalizer AbpEnumLocalizer { get; set; } = default!;

    [Parameter]
    public TEntity Entity { get; set; } = default!;

    [Parameter]
    public ObjectExtensionPropertyInfo PropertyInfo { get; set; } = default!;

    [Parameter]
    public AbpMudBlazorMessageLocalizerHelper<TResourceType> LH { get; set; } = default!;

    [Parameter]
    public ExtensionPropertyModalType? ModalType { get; set; }

    protected virtual string? Validate(object? value)
    {
        var validationAttributes = PropertyInfo.GetValidationAttributes();
        var validationContext = new ValidationContext(Entity)
        {
            DisplayName = PropertyInfo.Name,
            MemberName = PropertyInfo.Name
        };

        foreach (var validationAttribute in validationAttributes)
        {
            var result = validationAttribute.GetValidationResult(value, validationContext);
            if (result == ValidationResult.Success || result == null)
            {
                continue;
            }

            var errorMessage = result.ErrorMessage;
            if (LH != null && errorMessage != null)
            {
                var formattedErrorMessage = MudValidationAttributeHelper.GetDefaultErrorMessage(validationAttribute);
                var errorMessageString = MudValidationAttributeHelper.RevertErrorMessagePlaceholders(formattedErrorMessage);
                errorMessage = LH.Localize(errorMessageString);
            }

            return errorMessage;
        }

        return null;
    }

    protected bool IsReadonlyField => ModalType is ExtensionPropertyModalType.EditModal && PropertyInfo.UI.EditModal.IsReadOnly;
}
