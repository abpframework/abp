using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

public class AbpDatePickerRange
{
    private readonly List<string> _dates = new List<string>();
    public string Label { get; set; } = default!;
    public IReadOnlyList<string> Dates => _dates;

    public AbpDatePickerRange()
    {
        
    }
    public AbpDatePickerRange(string label, DateTime start, DateTime end)
    {
        Label = label;
        AddDate(start);
        AddDate(end);
    }
    
    public AbpDatePickerRange(string label, DateTime date)
    {
        Label = label;
        AddDate(date);
    }
    
    public AbpDatePickerRange(string label, DateTimeOffset start, DateTimeOffset end)
    {
        Label = label;
        AddDate(start);
        AddDate(end);
    }
    
    public AbpDatePickerRange(string label, DateTimeOffset date)
    {
        Label = label;
        AddDate(date);
    }
    
    public AbpDatePickerRange(string label, string start, string end)
    {
        Label = label;
        AddDate(start);
        AddDate(end);
    }
    
    public AbpDatePickerRange(string label, string date)
    {
        Label = label;
        AddDate(date);
    }
    
    public void AddDate(string date)
    {
        _dates.Add(DateTime.Parse(date).ToString("O"));
    }
    
    public void AddDate(DateTime date)
    {
        _dates.Add(date.ToString("O"));
    }
    
    public void AddDate(DateTimeOffset date)
    {
        _dates.Add(date.ToString("O"));
    }
    
    public void AddDate(DateTime? date)
    {
        if (date.HasValue)
        {
            _dates.Add(date.Value.ToString("O"));
        }
    }
    
    public void AddDate(DateTimeOffset? date)
    {
        if (date.HasValue)
        {
            _dates.Add(date.Value.ToString("O"));
        }
    }
    
    public void AddDate(string date, string format)
    {
        _dates.Add(DateTime.ParseExact(date, format, null).ToString("O"));
    }
}