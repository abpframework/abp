using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayOrder : Attribute
{
    public static int Default = 10000;

    public int Number { get; set; }

    public DisplayOrder(int number)
    {
        Number = number;
    }
}
