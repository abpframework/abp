using System;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.MudBlazorUI.Components.ObjectExtending;

public partial class MudDateTimeExtensionProperty<TEntity, TResourceType>
    where TEntity : IHasExtraProperties
{
    protected DateTime? Value
    {
        get => PropertyInfo.GetInputValueOrDefault<DateTime?>(Entity.GetProperty(PropertyInfo.Name));
        set => Entity.SetProperty(PropertyInfo.Name, value, false);
    }

    protected DateTime? DateValue
    {
        get => Value?.Date;
        set
        {
            if (value.HasValue)
            {
                var time = Value?.TimeOfDay ?? TimeSpan.Zero;
                Value = value.Value.Date + time;
            }
            else
            {
                Value = null;
            }
        }
    }

    protected TimeSpan? TimeValue
    {
        get => Value?.TimeOfDay;
        set
        {
            if (Value.HasValue && value.HasValue)
            {
                Value = Value.Value.Date + value.Value;
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
