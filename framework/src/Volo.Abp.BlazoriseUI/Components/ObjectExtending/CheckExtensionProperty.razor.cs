using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class CheckExtensionProperty<TEntity, TResourceType> : ComponentBase
    where TEntity : IHasExtraProperties
{
    [Inject]
    public IStringLocalizerFactory StringLocalizerFactory { get; set; }

    [Parameter]
    public TEntity Entity { get; set; }

    [Parameter]
    public ObjectExtensionPropertyInfo PropertyInfo { get; set; }

    protected bool Value
    {
        get
        {
            return PropertyInfo.GetInputValueOrDefault<bool>(Entity.GetProperty(PropertyInfo.Name));
        }
        set
        {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
