using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
