using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;
using Volo.Abp.Guids;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.TagHelpers;

public class AbpComponentDemoSectionTagHelper : AbpTagHelper
{
    private const string DemoSectionOpeningTag = "<abp-component-demo-section";
    private const string DemoSectionClosingTag = "</abp-component-demo-section";

    public string ViewPath { get; set; }
    public string Title { get; set; }

    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly IGuidGenerator _guidGenerator;

    public AbpComponentDemoSectionTagHelper(
        IVirtualFileProvider virtualFileProvider,
        IGuidGenerator guidGenerator)
    {
        _virtualFileProvider = virtualFileProvider;
        _guidGenerator = guidGenerator;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;

        var content = await output.GetChildContentAsync();

        var previewId = _guidGenerator.Create();
        var codeBlockId = _guidGenerator.Create();

        var codeBlockTabId = _guidGenerator.Create();
        var tagHelperCodeBlockId = _guidGenerator.Create();
        var bootstrapCodeBlockId = _guidGenerator.Create();

        output.PreContent.AppendHtml("<div class=\"card\">");
        output.PreContent.AppendHtml("<div class=\"card-header\">");
        output.PreContent.AppendHtml("<div class=\"row\">");
        output.PreContent.AppendHtml($"<div class=\"col\"><h4 class=\"card-title my-1\">{Title}</h4></div>");
        output.PreContent.AppendHtml("<div class=\"col-auto\">");
        output.PreContent.AppendHtml("<nav>");
        output.PreContent.AppendHtml("<div class=\"nav nav-pills\" role=\"tablist\">");
        output.PreContent.AppendHtml($"<a class=\"nav-item nav-link active\" id=\"nav-preview-tab-{previewId}\" data-bs-toggle=\"tab\" href=\"#nav-preview-{previewId}\" role=\"tab\" aria-controls=\"nav-preview-{previewId}\" aria-selected=\"true\">Preview</a>");
        output.PreContent.AppendHtml($"<a class=\"nav-item nav-link\" id=\"nav-code-tab-{codeBlockId}\" data-bs-toggle=\"tab\" href=\"#nav-code-{codeBlockId}\" role=\"tab\" aria-controls=\"nav-code-{codeBlockId}\" aria-selected=\"false\">Code</a>");
        output.PreContent.AppendHtml("</div>");
        output.PreContent.AppendHtml("</nav>");
        output.PreContent.AppendHtml("</div>"); // col-auto
        output.PreContent.AppendHtml("</div>"); // row
        output.PreContent.AppendHtml("</div>"); // card-header
        output.PreContent.AppendHtml("<div class=\"card-body\">");
        output.PreContent.AppendHtml($"<div class=\"tab-content\" id=\"nav-preview-tabContent-{previewId}\">");
        output.PreContent.AppendHtml($"<div class=\"tab-pane fade show active\" id=\"nav-preview-{previewId}\" role=\"tabpanel\" aria-labelledby=\"nav-preview-tab-{previewId}\">");

        /* component rendering here */

        output.PostContent.AppendHtml("</div>"); // tab-pane
        output.PostContent.AppendHtml($"<div class=\"tab-pane fade\" id=\"nav-code-{codeBlockId}\" role=\"tabpanel\" aria-labelledby=\"nav-code-tab-{codeBlockId}\">");

        /* CodeBlock tabs */

        output.PostContent.AppendHtml($"<ul class=\"nav nav-tabs\" id=\"code-block-tab-{codeBlockTabId}\" role=\"tablist\">");
        output.PostContent.AppendHtml("<li class=\"nav-item\">");
        output.PostContent.AppendHtml($"<a class=\"nav-link active\" id=\"tag-helper-tab-{tagHelperCodeBlockId}\" data-bs-toggle=\"pill\" href=\"#tag-helper-{tagHelperCodeBlockId}\" role=\"tab\" aria-controls=\"tag-helper-{tagHelperCodeBlockId}\" aria-selected=\"true\">Abp Tag Helper</a>");
        output.PostContent.AppendHtml("</li>");
        output.PostContent.AppendHtml("<li class=\"nav-item\">");
        output.PostContent.AppendHtml($"<a class=\"nav-link\" id=\"bootstrap-tab-{bootstrapCodeBlockId}\" data-bs-toggle=\"pill\" href=\"#bootstrap-{bootstrapCodeBlockId}\" role=\"tab\" aria-controls=\"bootstrap-{tagHelperCodeBlockId}\" aria-selected=\"true\">Bootstrap</a>");
        output.PostContent.AppendHtml("</li>");
        output.PostContent.AppendHtml("</ul>");
        output.PostContent.AppendHtml($"<div class=\"tab-content\" id=\"code-block-tabContent-{codeBlockTabId}\">");

        output.PostContent.AppendHtml($"<div class=\"tab-pane fade show active\" id=\"tag-helper-{tagHelperCodeBlockId}\" role=\"tabpanel\" aria-labelledby=\"tag-helper-tab-{tagHelperCodeBlockId}\">");
        output.PostContent.AppendHtml("<pre class=\"p-4\">");
        output.PostContent.AppendHtml("<code class=\"language-html\">");
        output.PostContent.Append(GetRawDemoSource());
        output.PostContent.AppendHtml("</code>");
        output.PostContent.AppendHtml("</pre>");
        output.PostContent.AppendHtml("</div>");

        output.PostContent.AppendHtml($"<div class=\"tab-pane fade\" id=\"bootstrap-{bootstrapCodeBlockId}\" role=\"tabpanel\" aria-labelledby=\"bootstrap-tab-{bootstrapCodeBlockId}\">");
        output.PostContent.AppendHtml("<pre class=\"p-4\">");
        output.PostContent.AppendHtml("<code class=\"language-html\">");
        output.PostContent.Append(content.GetContent());
        output.PostContent.AppendHtml("</code>");
        output.PostContent.AppendHtml("</pre>");
        output.PostContent.AppendHtml("</div>");

        output.PostContent.AppendHtml("</div>"); // tab-content
        output.PostContent.AppendHtml("</div>"); // tab-pane
        output.PostContent.AppendHtml("</div>"); // tab-content
        output.PostContent.AppendHtml("</div>"); // card-body
        output.PostContent.AppendHtml("</div>"); // card
    }

    private string GetRawDemoSource()
    {
        StringBuilder sourceBuilder = null;

        var lines = GetFileContent().SplitToLines();

        foreach (var line in lines)
        {
            if (line.Contains(DemoSectionOpeningTag) && GetName(line) == Title)
            {
                sourceBuilder = new StringBuilder();
            }
            else if (line.Contains(DemoSectionClosingTag, StringComparison.InvariantCultureIgnoreCase))
            {
                if (sourceBuilder == null)
                {
                    continue;
                }

                return sourceBuilder.ToString();
            }
            else
            {
                sourceBuilder?.AppendLine(line);
            }
        }

        throw new AbpException($"Could not find {Title} demo section inside {ViewPath}");
    }

    private string GetFileContent()
    {
        var viewFileInfo = _virtualFileProvider.GetFileInfo(ViewPath);
        return viewFileInfo.ReadAsString();
    }

    private string GetName(string line)
    {
        var str = line.Substring(line.IndexOf("title=\"", StringComparison.OrdinalIgnoreCase) + "title=\"".Length);
        str = str.Left(str.IndexOf("\"", StringComparison.OrdinalIgnoreCase));
        return str;
    }
}
