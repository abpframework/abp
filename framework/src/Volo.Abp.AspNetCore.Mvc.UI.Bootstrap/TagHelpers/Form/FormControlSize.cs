using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormControlSize : Attribute
    {
        public AbpFormControlSize Size { get; set; }

        public FormControlSize(AbpFormControlSize size)
        {
            Size = size;
        }
    }
}
