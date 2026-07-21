using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

public static class TagHelperOutputExtensions
{
    // ASCII whitespace per the HTML5 spec, used to tokenize space-separated
    // attribute values such as aria-describedby.
    private static readonly char[] HtmlAsciiWhitespace = { ' ', '\t', '\n', '\r', '\f' };

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
            .FirstOrDefault(a => string.Equals(a.Name, "aria-describedby", StringComparison.OrdinalIgnoreCase))
            ?.Value?.ToString();

        if (string.IsNullOrWhiteSpace(existing))
        {
            output.Attributes.SetAttribute("aria-describedby", token);
            return;
        }

        var tokens = existing.Split(HtmlAsciiWhitespace, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Any(t => t == token))
        {
            return;
        }

        output.Attributes.SetAttribute("aria-describedby", existing + " " + token);
    }
}
