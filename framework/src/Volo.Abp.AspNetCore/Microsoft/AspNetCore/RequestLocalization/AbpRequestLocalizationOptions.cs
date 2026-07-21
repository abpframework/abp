using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.RequestLocalization;

public class AbpRequestLocalizationOptions
{
    public List<Func<IServiceProvider, RequestLocalizationOptions, Task>> RequestLocalizationOptionConfigurators { get; }

    /// <summary>
    /// Enables culture detection from route data (e.g. /{culture}/page).
    /// Default value: false.
    /// </summary>
    public bool UseRouteBasedCulture { get; set; }

    public AbpRequestLocalizationOptions()
    {
        RequestLocalizationOptionConfigurators = new List<Func<IServiceProvider, RequestLocalizationOptions, Task>>();
    }
}
