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

        MenuItemCulturePrefixHelper.PrependCulturePrefix(context.Menu, "/" + culture);

        return Task.CompletedTask;
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
