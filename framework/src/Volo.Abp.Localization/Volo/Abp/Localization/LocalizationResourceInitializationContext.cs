using System;

namespace Volo.Abp.Localization;

public class LocalizationResourceInitializationContext
{
    public LocalizationResourceBase Resource { get; }

    public IServiceProvider ServiceProvider { get; }

    public LocalizationResourceInitializationContext(LocalizationResourceBase resource, IServiceProvider serviceProvider)
    {
        Resource = resource;
        ServiceProvider = serviceProvider;
    }
}
