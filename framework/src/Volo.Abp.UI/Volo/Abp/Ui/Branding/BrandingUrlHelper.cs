using System;

namespace Volo.Abp.Ui.Branding;

public static class BrandingUrlHelper
{
    // "//host/" and any url with a scheme are used as they are, the others are application relative.
    public static bool IsExternalUrl(string? url)
    {
        if (url.IsNullOrWhiteSpace())
        {
            return false;
        }

        var brandingUrl = url!.Trim();

        return brandingUrl.StartsWith("//", StringComparison.Ordinal) || HasScheme(brandingUrl);
    }

    public static string RemoveApplicationRelativePrefix(string url)
    {
        return url.StartsWith("~/", StringComparison.Ordinal)
            ? url.Substring(2)
            : url.TrimStart('/');
    }

    // Rendered into url('...') inside a style element and must not be able to end either of them.
    public static string EscapeCssValue(string url)
    {
        return url
            .Replace("\\", "\\\\")
            .Replace("'", "\\'")
            .Replace("<", "%3C")
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\f", string.Empty);
    }

    // A broken value like "http://[" also has a scheme, so it is matched here instead of by Uri.TryCreate.
    private static bool HasScheme(string url)
    {
        var schemeLength = url.IndexOf(':');
        if (schemeLength < 1 || !IsLetter(url[0]))
        {
            return false;
        }

        for (var i = 1; i < schemeLength; i++)
        {
            var character = url[i];
            if (!IsLetter(character) && !IsDigit(character) && character != '+' && character != '-' && character != '.')
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsLetter(char character)
    {
        return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
    }

    private static bool IsDigit(char character)
    {
        return character >= '0' && character <= '9';
    }
}
