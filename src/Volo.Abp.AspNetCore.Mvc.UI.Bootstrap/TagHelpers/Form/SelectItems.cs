using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectItems: Attribute
    {
        public IEnumerable<SelectListItem> Items { get; set; }

        public SelectItems(IEnumerable<SelectListItem> items)
        {
            Items = items;
        }
    }
}
