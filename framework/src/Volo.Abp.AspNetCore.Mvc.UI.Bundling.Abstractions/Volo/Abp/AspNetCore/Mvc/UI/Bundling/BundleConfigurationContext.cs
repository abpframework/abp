using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfigurationContext : IBundleConfigurationContext
    {
        public List<string> Files { get; }

        public IFileProvider FileProvider { get; }

        public IServiceProvider ServiceProvider { get; }

        public IAbpLazyServiceProvider LazyServiceProvider { get; }

        public BundleConfigurationContext(IServiceProvider serviceProvider, IFileProvider fileProvider)
        {
            Files = new List<string>();
            ServiceProvider = serviceProvider;
            LazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();
            FileProvider = fileProvider;
        }
    }
}
