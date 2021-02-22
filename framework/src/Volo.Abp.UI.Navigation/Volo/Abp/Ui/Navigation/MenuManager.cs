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
            var requiredPermissionMenus = new List<ApplicationMenuItem>();
            GetRequiredPermissionNameMenus(menuWithItems, requiredPermissionMenus);

            if (requiredPermissionMenus.Any())
            {
                var permissionChecker = serviceProvider.GetRequiredService<IPermissionChecker>();
                var grantResult = await permissionChecker.IsGrantedAsync(requiredPermissionMenus.Select(x => x.RequiredPermissionName).ToArray());

                var toBeDeletedMenus = new List<ApplicationMenuItem>();
                foreach (var menu in requiredPermissionMenus)
                {
                    if (grantResult.Result[menu.RequiredPermissionName!] != PermissionGrantResult.Granted)
                    {
                        toBeDeletedMenus.Add(menu);
                    }
                }

                RemoveMenus(menuWithItems, toBeDeletedMenus);
            }
        }

        protected virtual void GetRequiredPermissionNameMenus(IHasMenuItems menuWithItems, List<ApplicationMenuItem> output)
        {
            foreach (var menuItem in menuWithItems.Items)
            {
                if (!menuItem.RequiredPermissionName.IsNullOrWhiteSpace())
                {
                    output.Add(menuItem);
                }

                GetRequiredPermissionNameMenus(menuItem, output);
            }
        }

        protected virtual void RemoveMenus(IHasMenuItems menuWithItems, List<ApplicationMenuItem> toBeDeleted)
        {
            menuWithItems.Items.RemoveAll(toBeDeleted.Contains);

            foreach (var menuItem in menuWithItems.Items)
            {
                RemoveMenus(menuItem, toBeDeleted);
            }
        }

        protected virtual void NormalizeMenu(IHasMenuItems menuWithItems)
        {
            foreach (var menuItem in menuWithItems.Items)
            {
                NormalizeMenu(menuItem);
            }

            menuWithItems.Items.Normalize();
        }
    }
}
