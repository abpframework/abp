using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Docs.Documents;
using Volo.Docs.Localization;
using Volo.Docs.Utils;

namespace Volo.Docs.Areas.Documents.TagHelpers
{
    //TODO: Better to convert element "document-nav-tree"
    //TODO: Or better to convert to a partial view or to a view component instead of a tag helper!
    [HtmlTargetElement("ul", Attributes = "root-node")]
    public class TreeTagHelper : TagHelper
    {
        private readonly DocsUiOptions _uiOptions;

        private readonly IStringLocalizer<DocsResource> _localizer;

        private const string LiItemTemplateWithLink = @"<li class='{0}'><span class='plus-icon'><i class='fa fa-{1}'></i></span>{2}{3}</li>";

        private const string ListItemAnchor = @"<a href='{0}' class='{1}'>{2}</a>";

        private const string ListItemSpan = @"<span class='{0}'>{1}</span>";

        private const string UlItemTemplate = @"<ul class='nav nav-list tree' style='{1}'>{0}</ul>";

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

        [HtmlAttributeName("language")]
        public string LanguageCode { get; set; }

        public TreeTagHelper(IOptions<DocsUiOptions> urlOptions, IStringLocalizer<DocsResource> localizer)
        {
            _localizer = localizer;
            _uiOptions = urlOptions.Value;
        }

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
            var sb = new StringBuilder();
            
            var textCss = node.Path.IsNullOrEmpty() ? "tree-toggle" : "";
            var isNodeSelected = node.IsSelected(SelectedDocumentName);
            var listItemCss = node.HasChildItems ? "nav-header" : "last-link";
            if (isNodeSelected)
            {
                listItemCss += " selected-tree";
            }

            string listInnerItem;
            if (node.Path.IsNullOrEmpty() && node.IsLeaf)
            {
                listInnerItem = string.Format(ListItemSpan, textCss, node.Text.IsNullOrEmpty() ? "?" : node.Text);
            }
            else
            {
                var badgeStringBuilder = new StringBuilder();

                if (!node.Path.IsNullOrWhiteSpace() && node.CreationTime.HasValue && node.LastUpdatedTime.HasValue)
                {
                    if(node.CreationTime + TimeSpan.FromDays(14) > DateTime.Now)
                    {
                        var newBadge = sb.Append("<span class='badge badge-primary ms-2' title=\"").Append(_localizer["NewExplanation"]).Append("\">").Append(_localizer["New"]).Append("</span>").ToString();
                        
                        badgeStringBuilder.Append(newBadge);
                    }
                    else if (node.LastSignificantUpdateTime != null && node.LastSignificantUpdateTime + TimeSpan.FromDays(14) > DateTime.Now)
                    {
                        var updBadge = sb.Append("<span class='badge badge-light ms-2' title=\"").Append(_localizer["UpdatedExplanation"]).Append("\">").Append(_localizer["Upd"]).Append("</span>").ToString();
                        badgeStringBuilder.Append(updBadge);
                    }
                }

                sb.Clear();

                listInnerItem = string.Format(ListItemAnchor, NormalizePath(node.Path), textCss,
                    node.Text.IsNullOrEmpty()
                        ? "?"
                        : sb.Append(node.Text).Append(badgeStringBuilder.ToString()).ToString());
            }

            sb.Clear();
            
            return string.Format(LiItemTemplateWithLink,
                listItemCss,
                node.HasChildItems ? "chevron-right" : sb.Append("long-arrow-right ").Append(node.Path.IsNullOrEmpty() ?  "no-link" : "has-link").ToString(),
                listInnerItem,
                content);
        }

        private string NormalizePath(string path)
        {
            if (UrlHelper.IsExternalLink(path))
            {
                return path;
            }

            var pathWithoutFileExtension = RemoveFileExtensionFromPath(path);

            if (string.IsNullOrWhiteSpace(path))
            {
                return "javascript:;";
            }

            var prefix = _uiOptions.RoutePrefix;
            
            var sb = new StringBuilder();
            return sb.Append(prefix).Append(LanguageCode).Append("/").Append(ProjectName).Append("/").Append(Version).Append("/").Append(pathWithoutFileExtension).ToString();
        }

        private string RemoveFileExtensionFromPath(string path)
        {
            if (path == null)
            {
                return null;
            }

            return path.EndsWith("." + ProjectFormat)
                 ? path.Left(path.Length - ProjectFormat.Length - 1)
                 : path;
        }
    }
}