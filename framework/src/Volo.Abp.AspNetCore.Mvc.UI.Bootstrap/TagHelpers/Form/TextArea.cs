using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TextArea : Attribute
    {
        public int Rows { get; set; } = -1;

        public int Cols { get; set; } = -1;

        public TextArea()
        {
        }
    }
}
