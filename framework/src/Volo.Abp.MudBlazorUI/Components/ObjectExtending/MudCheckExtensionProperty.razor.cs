using Volo.Abp.Data;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudCheckExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected bool Value
    {
        get => Entity.GetProperty<bool>(PropertyInfo.Name);
        set => Entity.SetProperty(PropertyInfo.Name, value, validate: false);
    }
}
