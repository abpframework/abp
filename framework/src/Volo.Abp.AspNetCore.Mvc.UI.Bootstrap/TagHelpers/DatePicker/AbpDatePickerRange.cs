using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.DatePicker;

public class AbpDatePickerRange
{
    private readonly List<string> _dates = new List<string>();
    public string Label { get; set; }
    public IReadOnlyList<string> Dates => _dates;
    
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