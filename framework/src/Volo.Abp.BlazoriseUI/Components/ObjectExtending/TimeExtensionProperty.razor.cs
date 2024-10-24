using System;
using Volo.Abp.Data;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class TimeExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected TimeSpan? Value {
        get {
            return PropertyInfo.GetInputValueOrDefault<TimeSpan?>(Entity.GetProperty(PropertyInfo.Name));
        }
        set {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
