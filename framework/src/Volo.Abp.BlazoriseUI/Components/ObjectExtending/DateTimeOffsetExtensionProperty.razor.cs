using System;
using Volo.Abp.Data;

namespace Volo.Abp.BlazoriseUI.Components.ObjectExtending;

public partial class DateTimeOffsetExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected DateTimeOffset? Value {
        get {
            var raw = Entity.GetProperty(PropertyInfo.Name);
            return raw switch
            {
                null => null,
                DateTimeOffset dto => dto,
                DateTime dt => dt.Kind switch
                {
                    DateTimeKind.Utc => new DateTimeOffset(dt, TimeSpan.Zero),
                    DateTimeKind.Local => new DateTimeOffset(dt),
                    _ => new DateTimeOffset(DateTime.SpecifyKind(dt, DateTimeKind.Utc), TimeSpan.Zero)
                },
                _ => null
            };
        }
        set {
            Entity.SetProperty(PropertyInfo.Name, value, false);
        }
    }
}
