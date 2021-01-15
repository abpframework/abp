using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfigurationContext : IBundleConfigurationContext
    {
        public List<string> Files { get; }

        public IFileProvider FileProvider { get; }

        public IServiceProvider ServiceProvider { get; }

        private readonly IAbpLazyServiceProvider _lazyServiceProvider;

        public BundleConfigurationContext(IServiceProvider serviceProvider, IFileProvider fileProvider)
        {
            Files = new List<string>();
            ServiceProvider = serviceProvider;
            _lazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();
            FileProvider = fileProvider;
        }

        public AbpLocalizationOptions LocalizationOptions => _lazyServiceProvider.LazyGetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
    }
}
