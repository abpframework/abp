using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;
using System.Text.Encodings.Web;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

public static class TagHelperOutputExtensions
{
    public static string Render(this TagHelperOutput output, HtmlEncoder htmlEncoder)
    {
        using (var writer = new StringWriter())
        {
            output.WriteTo(writer, htmlEncoder);
            return writer.ToString();
        }
    }
}
