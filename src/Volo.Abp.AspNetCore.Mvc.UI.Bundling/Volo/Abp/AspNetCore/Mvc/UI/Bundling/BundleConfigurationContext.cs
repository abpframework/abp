using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfigurationContext : IBundleConfigurationContext
    {
        public List<string> Files { get; }

        public IServiceProvider ServiceProvider { get; }

        public IFileProvider FileProvider { get; }

        public BundleConfigurationContext(IServiceProvider serviceProvider, IFileProvider fileProvider)
        {
            Files = new List<string>();
            ServiceProvider = serviceProvider;
            FileProvider = fileProvider;
        }
    }
}