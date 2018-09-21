using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Docs.Documents;

namespace Volo.Docs.Areas.Documents.Helpers.TagHelpers
{
    [HtmlTargetElement("ul", Attributes = "root-node")]
    public class TreeTagHelper : TagHelper
    {
        private const string LiItemTemplate = @"<li>
                                                  <label class='tree-toggle {3}'>
                                                      <span class='plus-icon'>
                                                            <i class='fa fa-{4}'></i>
                                                      </span>
                                                  </label>
                                                  <a href='{0}' class='{5}'>{1}</a>
                                                {2}</li>";

        private const string UlItemTemplate = @"<ul class='nav nav-list tree'>
                                                    {0}
                                                </ul>";

        [HtmlAttributeName("root-node")]
        public NavigationNode RootNode { get; set; }

        [HtmlAttributeName("version")]
        public string Version { get; set; }

        [HtmlAttributeName("project-name")]
        public string ProjectName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = new StringBuilder();
            RootNode.Items?.ForEach(childNode =>
                {
                    content.Append(RenderTreeNodeAsHtml(childNode));
                });

            output.Content.AppendHtml(content.ToString());
        }

        private string RenderTreeNodeAsHtml(NavigationNode node)
        {
            var content = "";

            node.Items?.ForEach(innerNode =>
            {
                content += GetParentNode(innerNode);
            });

            return node.IsEmpty ?
                content :
                GetLeafNode(node, content);
        }

        private string GetParentNode(NavigationNode node)
        {
            return string.Format(UlItemTemplate, RenderTreeNodeAsHtml(node));
        }

        private string GetLeafNode(NavigationNode node, string content)
        {
            return string.Format(LiItemTemplate,
                node.Path.IsNullOrEmpty() ? "#" : "/documents/" + ProjectName + "/" + Version + "/" + node.Path,
                node.Text.IsNullOrEmpty() ? "?" : node.Text,
                content,
                node.HasChildItems ? "nav-header" : "last-link",
                node.HasChildItems ? "chevron-down" : "long-arrow-right",
                node.Path.IsNullOrEmpty() ? "tree-toggle" : "");
        }

    }
}
