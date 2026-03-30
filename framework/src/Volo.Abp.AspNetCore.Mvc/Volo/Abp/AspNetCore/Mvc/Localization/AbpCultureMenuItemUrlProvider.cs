using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

/// <summary>
/// Prepends the culture route prefix to all local menu item URLs
/// when the current request has a {culture} route value.
/// Only activates when <see cref="AbpRequestLocalizationOptions.UseRouteBasedCulture"/> is <c>true</c>.
/// </summary>
public class AbpCultureMenuItemUrlProvider : IMenuItemUrlProvider
{
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected IOptions<AbpRequestLocalizationOptions> LocalizationOptions { get; }
    protected IOptions<AbpLocalizationOptions> AbpLocalizationOptions { get; }

    public AbpCultureMenuItemUrlProvider(
        IHttpContextAccessor httpContextAccessor,
        IOptions<AbpRequestLocalizationOptions> localizationOptions,
        IOptions<AbpLocalizationOptions> abpLocalizationOptions)
    {
        HttpContextAccessor = httpContextAccessor;
        LocalizationOptions = localizationOptions;
        AbpLocalizationOptions = abpLocalizationOptions;
    }

    public virtual Task HandleAsync(MenuItemUrlProviderContext context)
    {
        if (!LocalizationOptions.Value.UseRouteBasedCulture)
        {
            return Task.CompletedTask;
        }

        var culture = GetCulture();
        if (string.IsNullOrEmpty(culture))
        {
            return Task.CompletedTask;
        }

        var prefix = "/" + culture;
        PrependCulturePrefix(context.Menu, prefix);

        return Task.CompletedTask;
    }

    protected virtual string? GetCulture()
    {
        var httpContext = HttpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            // MVC, Razor Pages, or Blazor SSR — read from route data.
            // If no {culture} route value, the URL has no culture prefix → return null.
            return httpContext.GetRouteValue("culture")?.ToString();
        }

        // Blazor interactive circuits: HttpContext is null because there is
        // no active HTTP request. Fall back to CultureInfo.CurrentUICulture
        // which was set by the middleware during SSR and persisted in the circuit.
        var currentCulture = CultureInfo.CurrentUICulture.Name;
        var isKnownCulture = AbpLocalizationOptions.Value.Languages
            .Any(l => string.Equals(l.CultureName, currentCulture, StringComparison.OrdinalIgnoreCase));

        return isKnownCulture ? currentCulture : null;
    }

    protected virtual void PrependCulturePrefix(IHasMenuItems menuWithItems, string prefix)
    {
        foreach (var item in menuWithItems.Items)
        {
            if (item.Url != null && item.Url.StartsWith('/'))
            {
                item.Url = prefix + item.Url;
            }

            PrependCulturePrefix(item, prefix);
        }
    }
}
