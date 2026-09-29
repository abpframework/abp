using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Ui.Branding;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Branding;

public static class UrlHelperBrandingExtensions
{
    /// <summary>
    /// Resolves a branding url of <see cref="IBrandingProvider"/>: "logo.svg", "/logo.svg" and
    /// "~/logo.svg" all keep working under a non-root <see cref="Microsoft.AspNetCore.Http.HttpRequest.PathBase"/>.
    /// </summary>
    public static string? ResolveBrandingUrl(this IUrlHelper urlHelper, string? url)
    {
        if (url.IsNullOrWhiteSpace())
        {
            return null;
        }

        var brandingUrl = url!.Trim();

        if (BrandingUrlHelper.IsExternalUrl(brandingUrl))
        {
            return brandingUrl;
        }

        var relativeUrl = BrandingUrlHelper.RemoveApplicationRelativePrefix(brandingUrl);

        // "/http://host/logo.svg" would become a local path that does not exist.
        if (BrandingUrlHelper.IsExternalUrl(relativeUrl))
        {
            return brandingUrl;
        }

        return urlHelper.Content("~/" + relativeUrl);
    }

    /// <summary>
    /// Same as <see cref="ResolveBrandingUrl"/>, escaped for url('...') in css.
    /// </summary>
    public static string? ResolveBrandingCssUrl(this IUrlHelper urlHelper, string? url)
    {
        var resolvedUrl = urlHelper.ResolveBrandingUrl(url);

        return resolvedUrl == null ? null : BrandingUrlHelper.EscapeCssValue(resolvedUrl);
    }
}
