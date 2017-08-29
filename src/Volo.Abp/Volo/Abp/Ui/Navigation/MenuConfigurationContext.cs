using System;

namespace Volo.Abp.Ui.Navigation
{
    public class MenuConfigurationContext : IMenuConfigurationContext
    {
        public ApplicationMenu Menu { get; set; }

        public IServiceProvider ServiceProvider { get; }

        public MenuConfigurationContext(ApplicationMenu menu, IServiceProvider serviceProvider)
        {
            Menu = menu;
            ServiceProvider = serviceProvider;
        }
    }
}