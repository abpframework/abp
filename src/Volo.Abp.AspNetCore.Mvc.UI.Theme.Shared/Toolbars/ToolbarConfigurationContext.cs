using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarConfigurationContext : IToolbarConfigurationContext
    {
        public Toolbar Toolbar { get; }

        public IServiceProvider ServiceProvider { get; }
        
        public ToolbarConfigurationContext(Toolbar toolbar, IServiceProvider serviceProvider)
        {
            Toolbar = toolbar;
            ServiceProvider = serviceProvider;
        }
    }
}