using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleConfigurationContext : IBundleConfigurationContext
    {
        public List<string> Files { get; }
        public ITheme Theme { get; }
        public IServiceProvider ServiceProvider { get; }

        public BundleConfigurationContext(List<string> files, ITheme theme, IServiceProvider serviceProvider)
        {
            Files = files;
            Theme = theme;
            ServiceProvider = serviceProvider;
        }
    }
}