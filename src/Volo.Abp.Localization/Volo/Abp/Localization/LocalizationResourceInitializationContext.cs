using System;

namespace Volo.Abp.Localization
{
    public class LocalizationResourceInitializationContext
    {
        public IServiceProvider ServiceProvider { get; }

        public LocalizationResourceInitializationContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}