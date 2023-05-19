using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class InputInfoText : Attribute
{
    public string Text { get; set; }

    public InputInfoText(string text)
    {
        Text = text;
    }
}
