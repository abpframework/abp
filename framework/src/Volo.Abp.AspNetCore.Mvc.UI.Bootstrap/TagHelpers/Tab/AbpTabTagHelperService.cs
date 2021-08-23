using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabTagHelperService : AbpTagHelperService<AbpTabTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();

            var childContent = await output.GetChildContentAsync();
            var tabHeader = GetTabHeaderItem(context, output);
            var tabContent = GetTabContentItem(context, output, childContent);

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);

            var active = TagHelper.Active ?? false;

            tabHeaderItems.Add(new TabItem(tabHeader, tabContent, active, TagHelper.Name, TagHelper.ParentDropdownName, false));

            output.SuppressOutput();
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = TagHelper.Name + "-tab";
            var link = TagHelper.Name;
            var control = TagHelper.Name;
            var title = TagHelper.Title;
            var attributes = GetTabHeaderAttributes(context, output);

            var classAttributesAsString = attributes.Where(a=>a.Name == "class").ToList().Select(a=>a.Value).JoinAsString(" ");
            var otherAttributes = attributes.Where(a => a.Name != "class").ToList();

            if (!string.IsNullOrWhiteSpace(TagHelper.ParentDropdownName))
            {
                var anchor = new TagBuilder("a");
                anchor.AddCssClass("dropdown-item " + classAttributesAsString);
                anchor.Attributes.Add("id", id);
                anchor.Attributes.Add("href", "#" + link);
                anchor.Attributes.Add("data-bs-toggle", "tab");
                anchor.Attributes.Add("role", "tab");
                anchor.Attributes.Add("aria-controls", control);
                anchor.Attributes.Add("aria-selected", "false");

                foreach (var attr in otherAttributes)
                {
                    anchor.Attributes.Add(attr.Name, attr.Value.ToString());
                }

                anchor.InnerHtml.AppendHtml(title);

                return anchor.ToHtmlString();
            }
            else
            {
                var anchor = new TagBuilder("a");
                anchor.AddCssClass("nav-link " + classAttributesAsString + " " + AbpTabItemActivePlaceholder);
                anchor.Attributes.Add("id", id);
                anchor.Attributes.Add("data-bs-toggle", TabItemsDataTogglePlaceHolder);
                anchor.Attributes.Add("href", "#" + link);
                anchor.Attributes.Add("role", "tab");
                anchor.Attributes.Add("aria-controls", control);
                anchor.Attributes.Add("aria-selected", AbpTabItemSelectedPlaceholder);

                foreach (var attr in otherAttributes)
                {
                    anchor.Attributes.Add(attr.Name, attr.Value.ToString());
                }

                anchor.InnerHtml.AppendHtml(title);

                var listItem = new TagBuilder("li");
                listItem.AddCssClass("nav-item");
                listItem.InnerHtml.AppendHtml(anchor);

                return listItem.ToHtmlString();
            }
        }

        protected virtual string GetTabContentItem(TagHelperContext context, TagHelperOutput output, TagHelperContent content)
        {
            var headerId = TagHelper.Name + "-tab";
            var id = TagHelper.Name;
            var attributes = GetTabContentAttributes(context, output);

            var classAttributesAsString = attributes.Where(a => a.Name == "class").ToList().Select(a => a.Name).JoinAsString(" ");
            var otherAttributes = attributes.Where(a => a.Name != "class").ToList();

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("tab-pane fade " + classAttributesAsString + " " + AbpTabItemShowActivePlaceholder);
            wrapper.Attributes.Add("id", id);
            wrapper.Attributes.Add("role", "tabpanel");
            wrapper.Attributes.Add("aria-labelledby", headerId);

            foreach (var attr in otherAttributes)
            {
                wrapper.Attributes.Add(attr.Name, attr.Value.ToString());
            }

            wrapper.InnerHtml.AppendHtml(content);

            return wrapper.ToHtmlString();
        }

        protected virtual void SetPlaceholderForNameIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Name))
            {
                TagHelper.Name = TabItemNamePlaceHolder;
            }
        }

        protected virtual List<TagHelperAttribute> GetTabContentAttributes(TagHelperContext context, TagHelperOutput output) {
            var contentprefix = "content-";
            return GetTabAttributesByPrefix(output.Attributes, contentprefix);
        }

        protected virtual List<TagHelperAttribute> GetTabHeaderAttributes(TagHelperContext context, TagHelperOutput output) {
            var headerprefix = "header-";
            return GetTabAttributesByPrefix(output.Attributes, headerprefix);
        }

        private List<TagHelperAttribute> GetTabAttributesByPrefix(TagHelperAttributeList attributes, string prefix) {
            return attributes.Where(a=>a.Name.StartsWith(prefix))
                .Select(a=> new TagHelperAttribute(a.Name.Substring(prefix.Length), a.Value)).ToList();
        }
    }
}
