using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming;

/// <summary>
/// Prepends the culture route prefix to menu item URLs in Blazor WebAssembly when route-based culture is enabled.
/// </summary>
public class AbpWasmCultureMenuItemUrlProvider : IMenuItemUrlProvider, ITransientDependency
{
    protected ICachedApplicationConfigurationClient ConfigurationClient { get; }
    protected IMenuItemCulturePrefixHelper MenuItemCulturePrefixHelper { get; }

    public AbpWasmCultureMenuItemUrlProvider(
        ICachedApplicationConfigurationClient configurationClient,
        IMenuItemCulturePrefixHelper menuItemCulturePrefixHelper)
    {
        ConfigurationClient = configurationClient;
        MenuItemCulturePrefixHelper = menuItemCulturePrefixHelper;
    }

    public virtual async Task HandleAsync(MenuItemUrlProviderContext context)
    {
        if (!OperatingSystem.IsBrowser())
        {
            return;
        }

        var config = await ConfigurationClient.GetAsync();
        if (!config.Localization.UseRouteBasedCulture)
        {
            return;
        }

        var culture = GetCulture(config);
        if (string.IsNullOrEmpty(culture))
        {
            return;
        }

        await MenuItemCulturePrefixHelper.PrependCulturePrefixAsync(context.Menu, "/" + culture);
    }

    protected virtual string? GetCulture(Mvc.ApplicationConfigurations.ApplicationConfigurationDto config)
    {
        var currentCulture = CultureInfo.CurrentCulture.Name;
        var languages = config.Localization.Languages;
        if (languages.Count == 0)
        {
            return null;
        }

        var isKnownCulture = languages
            .Any(l => string.Equals(l.CultureName, currentCulture, StringComparison.OrdinalIgnoreCase));

        return isKnownCulture ? currentCulture : null;
    }
}
