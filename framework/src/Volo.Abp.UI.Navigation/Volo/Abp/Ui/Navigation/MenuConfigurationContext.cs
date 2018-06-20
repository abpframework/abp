using System;

namespace Volo.Abp.UI.Navigation
{
    public class MenuConfigurationContext : IMenuConfigurationContext
    {
        public ApplicationMenu Menu { get; }

        public IServiceProvider ServiceProvider { get; }

        public MenuConfigurationContext(ApplicationMenu menu, IServiceProvider serviceProvider)
        {
            Menu = menu;
            ServiceProvider = serviceProvider;
        }
    }
}