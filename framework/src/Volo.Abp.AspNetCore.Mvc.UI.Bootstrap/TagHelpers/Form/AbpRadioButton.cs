using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AbpRadioButton : Attribute
    {
        public bool Inline { get; set; } = false;

        public bool Disabled { get; set; } = false;
    }
}
