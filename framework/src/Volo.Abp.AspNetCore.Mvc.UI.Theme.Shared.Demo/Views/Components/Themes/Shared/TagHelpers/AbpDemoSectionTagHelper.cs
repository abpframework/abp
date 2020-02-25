using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.TagHelpers
{
    public class AbpDemoSectionTagHelper : AbpTagHelper
    {
        private const string DemoSectionOpeningTag = "<abp-demo-section";
        private const string DemoSectionClosingTag = "</abp-demo-section";

        public string ViewPath { get; set; }

        public string Name { get; set; }

        private readonly IVirtualFileProvider _virtualFileProvider;

        public AbpDemoSectionTagHelper(IVirtualFileProvider virtualFileProvider)
        {
            _virtualFileProvider = virtualFileProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            var content = await output.GetChildContentAsync();

            output.PreContent.AppendHtml("<div class=\"abp-demo-section\">");
            output.PreContent.AppendHtml("<div class=\"abp-demo-section-body\">");

            /* component rendering here */

            output.PostContent.AppendHtml("</div>"); //abp-demo-section-body

            output.PostContent.AppendHtml("<div class=\"abp-demo-section-raw-source\">");
            output.PostContent.AppendHtml("<h3>ABP Tag Helpers</h3>");
            output.PostContent.AppendHtml("<pre>");
            output.PostContent.Append(GetRawDemoSource());
            output.PostContent.AppendHtml("</pre>");
            output.PostContent.AppendHtml("</div>"); //abp-demo-section-raw-source

            output.PostContent.AppendHtml("<div class=\"abp-demo-section-bs-source\">");
            output.PostContent.AppendHtml("<h3>Bootstrap</h3>");
            output.PostContent.AppendHtml("<pre>");
            output.PostContent.Append(content.GetContent());
            output.PostContent.AppendHtml("</pre>");
            output.PostContent.AppendHtml("</div>"); //abp-demo-section-bs-source

            output.PostContent.AppendHtml("</div>"); //abp-demo-section
        }

        private string GetRawDemoSource()
        {
            var viewFileInfo = _virtualFileProvider.GetFileInfo(ViewPath);
            var viewFileContent = viewFileInfo.ReadAsString();
            var lines = viewFileContent.SplitToLines();

            StringBuilder sb = null;

            foreach (var line in lines)
            {
                if (line.Contains(DemoSectionOpeningTag))
                {
                    if (GetName(line) == Name)
                    {
                        sb = new StringBuilder();
                    }
                }
                else if (line.Contains(DemoSectionClosingTag, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (sb == null)
                    {
                        continue;
                    }

                    return sb.ToString();
                }
                else if (sb != null)
                {
                    sb.AppendLine(line);
                }
            }

            return "";
        }

        private string GetName(string line)
        {
            var str = line.Substring(line.IndexOf("name=\"", StringComparison.OrdinalIgnoreCase) + "name=\"".Length);
            str = str.Left(str.IndexOf("\"", StringComparison.OrdinalIgnoreCase));
            return str;
        }
    }
}
