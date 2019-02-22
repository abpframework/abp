using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReadOnlyInput : Attribute
    {
        public bool PlainText { get; set; }

        public ReadOnlyInput()
        {
        }

        public ReadOnlyInput(bool plainText)
        {
            PlainText = plainText;
        }
    }
}
