using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
