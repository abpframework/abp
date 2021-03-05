using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.UI.Navigation
{
    public class MenuManager : IMenuManager, ITransientDependency
    {
        protected AbpNavigationOptions Options { get; }
        protected IHybridServiceScopeFactory ServiceScopeFactory { get; }

        public MenuManager(
            IOptions<AbpNavigationOptions> options,
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

                await CheckPermissionsAsync(scope.ServiceProvider, menu);
            }

            NormalizeMenu(menu);

            return menu;
        }

        protected virtual async Task CheckPermissionsAsync(IServiceProvider serviceProvider, IHasMenuItems menuWithItems)
        {
            var requiredPermissionItems = new List<ApplicationMenuItem>();
            GetRequiredPermissionNameMenus(menuWithItems, requiredPermissionItems);

            if (requiredPermissionItems.Any())
            {
                var permissionChecker = serviceProvider.GetRequiredService<IPermissionChecker>();
                var grantResult = await permissionChecker.IsGrantedAsync(requiredPermissionItems.Select(x => x.RequiredPermissionName).Distinct().ToArray());

                var toBeDeleted = new HashSet<ApplicationMenuItem>();
                foreach (var menu in requiredPermissionItems)
                {
                    if (grantResult.Result[menu.RequiredPermissionName!] != PermissionGrantResult.Granted)
                    {
                        toBeDeleted.Add(menu);
                    }
                }

                RemoveMenus(menuWithItems, toBeDeleted);
            }
        }

        protected virtual void GetRequiredPermissionNameMenus(IHasMenuItems menuWithItems, List<ApplicationMenuItem> output)
        {
            foreach (var item in menuWithItems.Items)
            {
                if (!item.RequiredPermissionName.IsNullOrWhiteSpace())
                {
                    output.Add(item);
                }

                GetRequiredPermissionNameMenus(item, output);
            }
        }

        protected virtual void RemoveMenus(IHasMenuItems menuWithItems, HashSet<ApplicationMenuItem> toBeDeleted)
        {
            menuWithItems.Items.RemoveAll(toBeDeleted.Contains);

            foreach (var item in menuWithItems.Items)
            {
                RemoveMenus(item, toBeDeleted);
            }
        }

        protected virtual void NormalizeMenu(IHasMenuItems menuWithItems)
        {
            foreach (var item in menuWithItems.Items)
            {
                NormalizeMenu(item);
            }

            menuWithItems.Items.Normalize();
        }
    }
}
