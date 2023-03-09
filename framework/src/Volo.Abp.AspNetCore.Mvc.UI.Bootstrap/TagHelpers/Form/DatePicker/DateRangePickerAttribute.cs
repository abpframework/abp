using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

[AttributeUsage(AttributeTargets.Property)]
public class DateRangePickerAttribute : Attribute
{
    public string PickerId { get; set; }

    public bool IsStart { get; set; }

    public bool IsEnd => !IsStart;

    public DateRangePickerAttribute(string pickerId, bool isStart = false)
    {
        PickerId = pickerId;
        IsStart = isStart;
    }
}