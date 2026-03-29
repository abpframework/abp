using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
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

    public AbpCultureMenuItemUrlProvider(
        IHttpContextAccessor httpContextAccessor,
        IOptions<AbpRequestLocalizationOptions> localizationOptions)
    {
        HttpContextAccessor = httpContextAccessor;
        LocalizationOptions = localizationOptions;
    }

    public virtual Task HandleAsync(MenuItemUrlProviderContext context)
    {
        if (!LocalizationOptions.Value.UseRouteBasedCulture)
        {
            return Task.CompletedTask;
        }

        var culture = HttpContextAccessor.HttpContext?.GetRouteValue("culture")?.ToString();
        if (string.IsNullOrEmpty(culture))
        {
            return Task.CompletedTask;
        }

        var prefix = "/" + culture;
        PrependCulturePrefix(context.Menu, prefix);

        return Task.CompletedTask;
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
