using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfigurationContext : IBundleConfigurationContext
    {
        public List<string> Files { get; }

        public IFileProvider FileProvider { get; }

        public BundleConfigurationContext(IServiceProvider serviceProvider, IFileProvider fileProvider)
        {
            Files = new List<string>();
            ServiceProvider = serviceProvider;
            FileProvider = fileProvider;
        }

        public IServiceProvider ServiceProvider { get; }
        private readonly object _serviceProviderLock = new object();

        private TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (_serviceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        private IOptions<AbpLocalizationOptions> _abpLocalizationOptions;

        public AbpLocalizationOptions LocalizationOptions =>
            LazyGetRequiredService(typeof(IOptions<AbpLocalizationOptions>), ref _abpLocalizationOptions).Value;
    }
}
