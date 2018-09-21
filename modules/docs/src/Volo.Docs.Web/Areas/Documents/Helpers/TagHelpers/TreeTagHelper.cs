using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Docs.Documents;

namespace Volo.Docs.Areas.Documents.Helpers.TagHelpers
{
    [HtmlTargetElement("ul", Attributes = "root-item")]
    public class TreeTagHelper : TagHelper
    {
        private const string LiItemTemplate = @"<li><label class='tree-toggle {3}'><span class='plus-icon'><i class='fa fa-chevron-down'></i></span></label><a href='{0}'>{1}</a>{2}</li>";
        private const string UlItemTemplate = @"<ul class='nav nav-list tree'>{0}</ul>";

        [HtmlAttributeName("root-item")]
        public NavigationNode RootItem { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendHtml(RenderTreeNodeAsHtml(RootItem));
        }

        private static string RenderTreeNodeAsHtml(NavigationNode node)
        {
            var childContent = "";

            if (node.Items != null && node.Items.Any())
            {
                node.Items.ForEach(innerNode =>
                {
                    childContent += string.Format(UlItemTemplate, RenderTreeNodeAsHtml(innerNode));
                });
            }

            if (node.IsEmpty)
            {
                return childContent;
            }

            return string.Format(LiItemTemplate,
                string.IsNullOrWhiteSpace(node.Path) ? "#" : node.Path,
                string.IsNullOrEmpty(node.Text) ? "?" : node.Text,
                childContent, node.HasChildItems ? "nav-header" : "last-link");
        }

    }
}
