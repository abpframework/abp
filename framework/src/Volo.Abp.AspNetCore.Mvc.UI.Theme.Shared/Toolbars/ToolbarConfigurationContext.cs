using System;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarConfigurationContext : IToolbarConfigurationContext
    {
        public ITheme Theme { get; }

        public Toolbar Toolbar { get; }

        public IServiceProvider ServiceProvider { get; }
        
        public ToolbarConfigurationContext(ITheme currentTheme, Toolbar toolbar, IServiceProvider serviceProvider)
        {
            Theme = currentTheme;
            Toolbar = toolbar;
            ServiceProvider = serviceProvider;
        }
    }
}