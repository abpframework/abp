using System.Linq;
using Blazorise;
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

        e.Status = ValidationStatus.Error;
        e.ErrorText = result.First().ErrorMessage;
    }
}
