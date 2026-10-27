using System;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Ui.Branding;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Branding;

public static class NavigationManagerBrandingExtensions
{
    /// <summary>
    /// Resolves a branding url of <see cref="IBrandingProvider"/>: "logo.svg", "/logo.svg" and
    /// "~/logo.svg" all keep working under a non-root base path.
    /// </summary>
    public static string? ResolveBrandingUrl(this NavigationManager navigationManager, string? url)
    {
        Check.NotNull(navigationManager, nameof(navigationManager));

        if (url.IsNullOrWhiteSpace())
        {
            return null;
        }

        var brandingUrl = url!.Trim();

        if (BrandingUrlHelper.IsExternalUrl(brandingUrl))
        {
            return brandingUrl;
        }

        var applicationRelativeUrl = BrandingUrlHelper.RemoveApplicationRelativePrefix(brandingUrl);
        var baseUri = new Uri(navigationManager.BaseUri);

        // "/http://host/logo.svg" would silently lose its host when only the path is taken.
        if (BrandingUrlHelper.IsExternalUrl(applicationRelativeUrl) ||
            !Uri.TryCreate(baseUri, applicationRelativeUrl, out var absoluteUrl) ||
            absoluteUrl.GetLeftPart(UriPartial.Authority) != baseUri.GetLeftPart(UriPartial.Authority))
        {
            return brandingUrl;
        }

        return absoluteUrl.PathAndQuery + absoluteUrl.Fragment;
    }

    /// <summary>
    /// Same as <see cref="ResolveBrandingUrl"/>, escaped for url('...') in css.
    /// </summary>
    public static string? ResolveBrandingCssUrl(this NavigationManager navigationManager, string? url)
    {
        var resolvedUrl = navigationManager.ResolveBrandingUrl(url);

        return resolvedUrl == null ? null : BrandingUrlHelper.EscapeCssValue(resolvedUrl);
    }
}
