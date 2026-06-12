using System;
using Volo.Abp.Data;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudTimeExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected TimeSpan? Value
    {
        get => PropertyInfo.GetInputValueOrDefault<TimeSpan?>(Entity.GetProperty(PropertyInfo.Name));
        set => Entity.SetProperty(PropertyInfo.Name, value, false);
    }
}
