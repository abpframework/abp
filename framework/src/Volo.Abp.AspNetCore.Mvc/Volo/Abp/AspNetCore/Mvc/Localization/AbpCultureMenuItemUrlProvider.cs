using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

/// <summary>
/// Prepends the culture route prefix to menu item URLs when route-based culture is enabled.
/// </summary>
public class AbpCultureMenuItemUrlProvider : IMenuItemUrlProvider, ITransientDependency
{
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected IOptions<AbpRequestLocalizationOptions> LocalizationOptions { get; }
    protected IOptions<AbpLocalizationOptions> AbpLocalizationOptions { get; }
    protected IMenuItemCulturePrefixHelper MenuItemCulturePrefixHelper { get; }

    public AbpCultureMenuItemUrlProvider(
        IHttpContextAccessor httpContextAccessor,
        IOptions<AbpRequestLocalizationOptions> localizationOptions,
        IOptions<AbpLocalizationOptions> abpLocalizationOptions,
        IMenuItemCulturePrefixHelper menuItemCulturePrefixHelper)
    {
        HttpContextAccessor = httpContextAccessor;
        LocalizationOptions = localizationOptions;
        AbpLocalizationOptions = abpLocalizationOptions;
        MenuItemCulturePrefixHelper = menuItemCulturePrefixHelper;
    }

    public virtual async Task HandleAsync(MenuItemUrlProviderContext context)
    {
        if (!LocalizationOptions.Value.UseRouteBasedCulture)
        {
            return;
        }

        var culture = GetCulture();
        if (string.IsNullOrEmpty(culture))
        {
            return;
        }

        await MenuItemCulturePrefixHelper.PrependCulturePrefixAsync(context.Menu, "/" + culture);
    }

    protected virtual string? GetCulture()
    {
        var httpContext = HttpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            return AbpRequestCultureCookieHelper.GetRouteCulture(httpContext);
        }

        // No HttpContext: fallback to CurrentCulture.
        var currentCulture = CultureInfo.CurrentCulture.Name;
        var isKnownCulture = AbpLocalizationOptions.Value.Languages
            .Any(l => string.Equals(l.CultureName, currentCulture, StringComparison.OrdinalIgnoreCase));

        return isKnownCulture ? currentCulture : null;
    }

}
