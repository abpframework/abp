using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.UI.Navigation
{
    public class MenuManager : IMenuManager, ITransientDependency
    {
        protected NavigationOptions Options { get; }
        protected IHybridServiceScopeFactory ServiceScopeFactory { get; }

        public MenuManager(
            IOptions<NavigationOptions> options, 
            IHybridServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Options = options.Value;
        }

        public async Task<ApplicationMenu> GetAsync(string name)
        {
            var menu = new ApplicationMenu(name);

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = new MenuConfigurationContext(menu, scope.ServiceProvider);

                foreach (var contributor in Options.MenuContributors)
                {
                    await contributor.ConfigureMenuAsync(context);
                }
            }

            NormalizeMenu(menu);

            return menu;
        }

        protected virtual void NormalizeMenu(ApplicationMenu menu)
        {
            //TODO: Should also consider sub menus, recursively, bottom to top!
            menu.Items.RemoveAll(item => item.IsLeaf && item.Url.IsNullOrEmpty());
        }
    }
}