using System;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudDateTimeOffsetExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected DateTimeOffset? Value
    {
        get
        {
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
        set => Entity.SetProperty(PropertyInfo.Name, value, false);
    }

    protected DateTime? DateOnlyValue
    {
        get => Value?.DateTime.Date;
        set
        {
            if (value.HasValue)
            {
                Value = new DateTimeOffset(value.Value);
            }
            else
            {
                Value = null;
            }
        }
    }

    protected DateTime? DateValue
    {
        get => Value?.DateTime.Date;
        set
        {
            if (value.HasValue)
            {
                var time = Value?.DateTime.TimeOfDay ?? TimeSpan.Zero;
                var offset = Value?.Offset ?? TimeSpan.Zero;
                Value = new DateTimeOffset(value.Value.Date + time, offset);
            }
            else
            {
                Value = null;
            }
        }
    }

    protected TimeSpan? TimeValue
    {
        get => Value?.DateTime.TimeOfDay;
        set
        {
            if (Value.HasValue && value.HasValue)
            {
                var offset = Value.Value.Offset;
                Value = new DateTimeOffset(Value.Value.DateTime.Date + value.Value, offset);
            }
        }
    }

    protected string GetDateFormat()
    {
        var dataFormatString = PropertyInfo.GetDataFormatStringOrNull();
        if (!string.IsNullOrEmpty(dataFormatString))
        {
            return dataFormatString.Replace("{0:", "").Replace("}", "");
        }

        return PropertyInfo.IsDate() ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm";
    }

    protected string GetTimeLabel()
    {
        return "Time";
    }
}
