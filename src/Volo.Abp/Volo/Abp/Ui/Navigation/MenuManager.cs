using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.DependencyInjection;

namespace Volo.Abp.Ui.Navigation
{
    public class MenuManager : IMenuManager, ITransientDependency
    {
        private readonly NavigationOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public MenuManager(
            IOptions<NavigationOptions> options, 
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public async Task<ApplicationMenu> GetAsync(string name)
        {
            var menu = new ApplicationMenu(name);

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new MenuConfigurationContext(menu, scope.ServiceProvider);

                foreach (var contributor in _options.MenuContributors)
                {
                    await contributor.ConfigureMenuAsync(context);
                }
            }

            return menu;
        }
    }
}