using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Components.TagHelperProperties;

public class TagHelperPropertiesViewComponent : AbpViewComponent
{
    public List<PropertyInfo> Properties { get; } = new();
    public IViewComponentResult Invoke(Type type)
    {
        foreach (var property in type.GetRuntimeProperties())
        {
            if (typeof(AbpTagHelper).IsAssignableFrom(property.DeclaringType) && 
                property.GetCustomAttribute<HtmlAttributeNotBoundAttribute>() == null && 
                !property.PropertyType.IsAbstract && 
                property.GetMethod?.IsPublic == true)
            {
                Properties.Add(property);
            }
        }

        return View("/Components/TagHelperProperties/Default.cshtml", Properties);
    }
}
