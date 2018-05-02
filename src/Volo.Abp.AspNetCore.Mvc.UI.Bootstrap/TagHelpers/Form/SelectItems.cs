using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectItems: Attribute
    {
        public string ItemsListPropertyName { get; set; }

        public SelectType SelectType { get; set; } = SelectType.Dropdown;

        public IEnumerable<SelectListItem> GetItems(ModelExplorer explorer, string selectedValue)
        {
            var properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();

            while (properties.Count == 0)
            {
                explorer = explorer.Container;
                properties = explorer.Container.Properties.Where(p => p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();

                if (explorer.Container == null)
                {
                    return null;
                }
            }

            var selectItems = (properties.First().Model as IEnumerable<SelectListItem>).ToList();
            
            return selectItems;
        }
    }
}
