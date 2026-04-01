using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class RouteBasedCultureUrlHelper : IRouteBasedCultureUrlHelper, ITransientDependency
{
    private readonly ICachedApplicationConfigurationClient _configurationClient;

    public RouteBasedCultureUrlHelper(ICachedApplicationConfigurationClient configurationClient)
    {
        _configurationClient = configurationClient;
    }

    public virtual async Task<string> PrependCulturePrefixAsync(string url)
    {
        var config = await _configurationClient.GetAsync();

        if (config?.Localization.UseRouteBasedCulture != true)
        {
            return url;
        }

        var currentCulture = CultureInfo.CurrentCulture.Name;
        var isKnownCulture = config.Localization.Languages
            .Any(l => string.Equals(l.CultureName, currentCulture, StringComparison.OrdinalIgnoreCase));

        return isKnownCulture ? $"{currentCulture}/{url}" : url;
    }
}
