using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class BooleanSelector : Attribute
{
    public string TrueText { get; set; }

    public string FalseText { get; set; }

    public string EmptyText { get; set; }

    public BooleanSelector(string emptyText = "", string falseText = "False", string trueText = "True")
    {
        EmptyText = emptyText;
        FalseText = falseText;
        TrueText = trueText;
    }

    public IEnumerable<SelectListItem> GetItems(ModelExplorer explorer)
    {
        var selectItems = new List<SelectListItem>();

        if (explorer.Metadata.IsNullableValueType)
        {
            selectItems.Add(new SelectListItem { Text = EmptyText });
        }

        selectItems.Add(new SelectListItem { Text = FalseText, Value = "False" });
        selectItems.Add(new SelectListItem { Text = TrueText, Value = "True" });

        return selectItems;
    }
}
