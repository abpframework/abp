using System;
using Volo.Abp.Data;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class DateTimeExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected DateTime? Value {
        get {
            return PropertyInfo.GetInputValueOrDefault<DateTime?>(Entity.GetProperty(PropertyInfo.Name));
        }
        set {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
