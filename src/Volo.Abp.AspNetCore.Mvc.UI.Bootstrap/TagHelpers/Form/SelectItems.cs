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

        public Type EnumType { get; set; }

        public SelectType SelectType { get; set; } = SelectType.Dropdown;

        public IEnumerable<SelectListItem> GetItems(ModelExplorer explorer)
        {
            if (IsEnumItem())
            {
                return GetItemsFromEnum();
            }
            else
            {
                return GetItemsFromListField(explorer);
            }
        }

        private IEnumerable<SelectListItem> GetItemsFromListField(ModelExplorer explorer)
        {
            var properties = explorer.Properties.Where(p => p.Metadata.PropertyName.Equals(ItemsListPropertyName)).ToList();

            return properties.Count > 0
                ? properties.First().Model as IEnumerable<SelectListItem>
                : new List<SelectListItem>();
        }

        private IEnumerable<SelectListItem> GetItemsFromEnum()
        {
            var enumItems = EnumType.GetTypeInfo().GetMembers(BindingFlags.Public | BindingFlags.Static);

            return enumItems.Select((t, i) => new SelectListItem() { Value = i.ToString(), Text = t.Name }).ToList();
        }

        private bool IsEnumItem()
        {
            return EnumType != null && EnumType.GetTypeInfo().IsEnum;
        }
    }
}
