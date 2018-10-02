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
                                                  <a href='{0}' class='{5} {3}'> 
                                                      <span class='plus-icon'><i class='fa fa-{4}'></i></span>{1}
                                                  </a>
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

        [HtmlAttributeName("project-format")]
        public string ProjectFormat { get; set; }

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

            var isAnyNodeOpenedInThisLevel = IsAnyNodeOpenedInThisLevel(node);

            node.Items?.ForEach(innerNode =>
            {
                content += GetParentNode(innerNode, isAnyNodeOpenedInThisLevel);
            });

            var result = node.IsEmpty ? content : GetLeafNode(node, content);

            return result;
        }

        private bool IsAnyNodeOpenedInThisLevel(NavigationNode node)
        {
            if (node.Items == null)
            {
                return false;
            }

            if (node.IsSelected(SelectedDocumentName))
            {
                return true;
            }

            if (node.Items.Any(n => n.IsSelected(SelectedDocumentName)))
            {
                return true;
            }

            return false;
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

            //if (isNodeSelected)
            //{
            //    anchorCss += " opened";
            //}

            var normalizedPath = node.Path != null && node.Path.EndsWith("." + ProjectFormat)
                ? node.Path.Left(node.Path.Length - ProjectFormat.Length - 1)
                : node.Path;

            return string.Format(LiItemTemplate,
                node.Path.IsNullOrEmpty() ? "javascript:;" : "/documents/" + ProjectName + "/" + Version + "/" + normalizedPath,
                node.Text.IsNullOrEmpty() ? "?" : node.Text,
                content,
                node.HasChildItems ? "nav-header" : "last-link",
                node.HasChildItems ? "chevron-right" : "long-arrow-right",
                anchorCss,
                isNodeSelected ? "selected-tree" : "");
        }

    }
}
