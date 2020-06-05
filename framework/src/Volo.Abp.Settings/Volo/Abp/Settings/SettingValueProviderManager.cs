using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings
{
    public class SettingValueProviderManager : ISettingValueProviderManager, ISingletonDependency
    {
        public List<ISettingValueProvider> Providers => _lazyProviders.Value;
        protected AbpSettingOptions Options { get; }
        private readonly Lazy<List<ISettingValueProvider>> _lazyProviders;

        public SettingValueProviderManager(
            IServiceProvider serviceProvider,
            IOptions<AbpSettingOptions> options)
        {

            Options = options.Value;

            _lazyProviders = new Lazy<List<ISettingValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(type => serviceProvider.GetRequiredService(type) as ISettingValueProvider)
                    .ToList(),
                true
            );
        }
    }
}