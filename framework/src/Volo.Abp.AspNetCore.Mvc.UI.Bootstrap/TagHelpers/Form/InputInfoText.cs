using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputInfoText : Attribute
    {
        public string Text { get; set; }

        public InputInfoText(string text)
        {
            Text = text;
        }
    }
}
