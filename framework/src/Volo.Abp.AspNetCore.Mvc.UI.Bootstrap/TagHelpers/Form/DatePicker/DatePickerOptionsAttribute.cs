using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

[AttributeUsage(AttributeTargets.Property)]
public class DatePickerOptionsAttribute : Attribute
{
    public string DatePickerOptionsPropertyName { get; set; }
    
    public DatePickerOptionsAttribute(string datePickerOptionsPropertyName)
    {
        DatePickerOptionsPropertyName = datePickerOptionsPropertyName;
    }
    
    public IAbpDatePickerOptions? GetDatePickerOptions(ModelExplorer explorer)
    {
        var properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName != null && p.Metadata.PropertyName.Equals(DatePickerOptionsPropertyName)).ToList();

        while (properties.Count == 0)
        {
            explorer = explorer.Container;
            if(explorer.Container == null)
            {
                return null;
            }
                
            properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName != null && p.Metadata.PropertyName.Equals(DatePickerOptionsPropertyName)).ToList();
        }
            
        return properties.FirstOrDefault(p=> p.Model is IAbpDatePickerOptions)?.Model as IAbpDatePickerOptions;
    }
}