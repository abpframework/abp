using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfigurationContext : IBundleConfigurationContext
    {
        public List<string> Files { get; }
        public IServiceProvider ServiceProvider { get; }

        public BundleConfigurationContext(IServiceProvider serviceProvider)
        {
            Files = new List<string>();
            ServiceProvider = serviceProvider;
        }
    }
}