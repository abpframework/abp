using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectItems: Attribute
    {
        public string ItemsListPropertyName { get; set; }

        public SelectType SelectType { get; set; } = SelectType.Dropdown;

        public IEnumerable<SelectListItem> GetItems(ModelExplorer explorer)
        {
            var properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();

            return properties.Count > 0
                ? properties.First().Model as IEnumerable<SelectListItem>
                : null;
        }
    }
}
