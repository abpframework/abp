using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisabledInput : Attribute
    {
        public DisabledInput()
        {
        }
    }
}
