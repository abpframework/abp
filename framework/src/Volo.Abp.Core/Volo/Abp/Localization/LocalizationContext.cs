using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization
{
    public class LocalizationContext : IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; }

        public IStringLocalizerFactory LocalizerFactory { get; }

        public LocalizationContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            LocalizerFactory = ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
        }
    }
}