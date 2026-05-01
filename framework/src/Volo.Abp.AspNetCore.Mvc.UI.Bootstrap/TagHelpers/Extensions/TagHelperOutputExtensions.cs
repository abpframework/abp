using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using System.Linq;
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

    /// <summary>
    /// Appends an id token to the space-separated <c>aria-describedby</c> attribute,
    /// preserving any tokens that were already present (e.g. provided by the consumer)
    /// and skipping the token when it is already in the list.
    /// </summary>
    public static void AppendAriaDescribedby(this TagHelperOutput output, string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        var existing = output.Attributes
            .FirstOrDefault(a => a.Name == "aria-describedby")?.Value?.ToString();

        if (string.IsNullOrEmpty(existing))
        {
            output.Attributes.SetAttribute("aria-describedby", token);
            return;
        }

        var tokens = existing.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Any(t => t == token))
        {
            return;
        }

        output.Attributes.SetAttribute("aria-describedby", existing + " " + token);
    }
}
