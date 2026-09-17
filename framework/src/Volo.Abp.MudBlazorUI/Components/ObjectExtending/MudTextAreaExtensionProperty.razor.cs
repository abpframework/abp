using Volo.Abp.Data;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudTextAreaExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected string? Value
    {
        get => PropertyInfo.GetTextInputValueOrNull(Entity.GetProperty(PropertyInfo.Name));
        set => Entity.SetProperty(PropertyInfo.Name, value, validate: false);
    }
}
