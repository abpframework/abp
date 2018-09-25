using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Docs.Documents;

namespace Volo.Docs.Areas.Documents.Helpers.TagHelpers
{
    [HtmlTargetElement("ul", Attributes = "root-node")]
    public class TreeTagHelper : TagHelper
    {
        private const string LiItemTemplate = @"<li class='{6}'>
                                                  <label class='tree-toggle {3}'>
                                                      <span class='plus-icon'>
                                                            <i class='fa fa-{4}'></i>
                                                      </span>
                                                  </label>
                                                  <a href='{0}' class='{5}'>{1}</a>
                                                  {2}
                                              </li>";

        private const string UlItemTemplate = @"<ul class='nav nav-list tree' style='{1}'>
                                                    {0}
                                                </ul>";

        [HtmlAttributeName("root-node")]
        public NavigationNode RootNode { get; set; }

        [HtmlAttributeName("version")]
        public string Version { get; set; }

        [HtmlAttributeName("project-name")]
        public string ProjectName { get; set; }

        [HtmlAttributeName("selected-document-name")]
        public string SelectedDocumentName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = new StringBuilder();

            RootNode.Items?.ForEach(childNode =>
            {
                content.Append(RenderNodeAsHtml(childNode));
            });

            output.Content.AppendHtml(content.ToString());
        }

        private string RenderNodeAsHtml(NavigationNode node)
        {
            var content = "";

            var isAnyNodeOpenedInThisLevel = node.Items?.Any(n => n.IsSelected(SelectedDocumentName)) ?? false;

            node.Items?.ForEach(innerNode =>
            {
                content += GetParentNode(innerNode, isAnyNodeOpenedInThisLevel);
            });

            var result = node.IsEmpty ? content : GetLeafNode(node, content);

            return result;
        }

        private string GetParentNode(NavigationNode node, bool isOpened)
        {
            var output = RenderNodeAsHtml(node);

            return string.Format(UlItemTemplate, output, isOpened ? "" : "display: none;");
        }

        private string GetLeafNode(NavigationNode node, string content)
        {
            var anchorCss = node.Path.IsNullOrEmpty() ? "tree-toggle" : "";
            var isNodeSelected = node.IsSelected(SelectedDocumentName);

            if (isNodeSelected)
            {
                anchorCss += " opened";
            }

            return string.Format(LiItemTemplate,
                node.Path.IsNullOrEmpty() ? "#" : "/documents/" + ProjectName + "/" + Version + "/" + node.Path,
                node.Text.IsNullOrEmpty() ? "?" : node.Text,
                content,
                node.HasChildItems ? "nav-header" : "last-link",
                node.HasChildItems ? "chevron-down" : "long-arrow-right",
                anchorCss ,
                isNodeSelected? "selected-tree" : "");
        }

    }
}
