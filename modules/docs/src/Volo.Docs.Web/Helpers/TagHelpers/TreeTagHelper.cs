using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Docs.Documents;

namespace Volo.Docs.Helpers.TagHelpers
{
    [HtmlTargetElement("ul", Attributes = "navigation-items")]
    public class TreeTagHelper : TagHelper
    {
        //private readonly IHttpContextAccessor _contextAccessor;
        private const string LiItemTemplate = @"<li><label class='tree-toggle nav-header'><span class='plus-icon'><i class='fa fa-chevron-down'></i></span></label><a href='{0}'>{1}</a>{2}</li>";
        private const string UlItemTemplate = @"<ul class='nav nav-list tree'>{0}</ul>";

        //public TreeTagHelper(IHttpContextAccessor contextAccessor)
        //{
        //    _contextAccessor = contextAccessor;
        //}

        [HtmlAttributeName("navigation-items")]
        public NavigationNode RootItem { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var rootUl = string.Format(UlItemTemplate, GetNodeHtml(RootItem));

            output.Content.AppendHtml(rootUl);
            //output.Attributes.SetAttribute("item-count", test);
        }

        private static string GetNodeHtml(NavigationNode node)
        {
            var childContent = "";

            if (node.Items != null && node.Items.Any())
            {
                node.Items.ForEach(innerNode =>
                {
                    childContent += string.Format(UlItemTemplate, GetNodeHtml(innerNode));
                });
            }

            var li = string.Format(LiItemTemplate, string.IsNullOrWhiteSpace(node.Path) ? "#" : node.Path, node.Text, childContent);

            return li;
        }

    }
}
