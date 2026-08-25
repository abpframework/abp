using System;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Branding;

public static class UrlHelperBrandingExtensions
{
    /// <summary>
    /// Resolves a branding url of <see cref="Volo.Abp.Ui.Branding.IBrandingProvider"/> for the current request.
    /// "logo.svg", "/logo.svg" and "~/logo.svg" all mean the same application relative url and keep working
    /// under a non-root <see cref="Microsoft.AspNetCore.Http.HttpRequest.PathBase"/>.
    /// External urls ("http://", "https://" and "//host/") are returned as they are.
    /// Returns null when <paramref name="url"/> is null or white space.
    /// </summary>
    public static string? ResolveBrandingUrl(this IUrlHelper urlHelper, string? url)
    {
        if (url.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (IsExternalUrl(url!))
        {
            return url;
        }

        var applicationRelativeUrl = url!.StartsWith("~/", StringComparison.Ordinal)
            ? url
            : "~/" + url.TrimStart('/');

        return urlHelper.Content(applicationRelativeUrl);
    }

    private static bool IsExternalUrl(string url)
    {
        return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
               || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
               || url.StartsWith("//", StringComparison.Ordinal);
    }
}
