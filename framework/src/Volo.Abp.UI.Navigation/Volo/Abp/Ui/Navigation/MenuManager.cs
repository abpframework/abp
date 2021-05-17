using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;
using Volo.Abp.Threading;

namespace Volo.Abp.UI.Navigation
{
    public class MenuManager : IMenuManager, ITransientDependency
    {
        protected AbpNavigationOptions Options { get; }
        protected IHybridServiceScopeFactory ServiceScopeFactory { get; }
        protected ISimpleStateCheckerManager<ApplicationMenuItem> SimpleStateCheckerManager { get; }
        protected SemaphoreSlim SyncSemaphore { get; }

        public MenuManager(
            IOptions<AbpNavigationOptions> options,
            IHybridServiceScopeFactory serviceScopeFactory,
            ISimpleStateCheckerManager<ApplicationMenuItem> simpleStateCheckerManager)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
            SimpleStateCheckerManager = simpleStateCheckerManager;
            SyncSemaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<ApplicationMenu> GetAsync(string name)
        {
            var menu = new ApplicationMenu(name);

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                using (await SyncSemaphore.LockAsync())
                {
                    RequirePermissionsSimpleBatchStateChecker<ApplicationMenuItem>.Instance.ClearCheckModels();

                    var context = new MenuConfigurationContext(menu, scope.ServiceProvider);

                    foreach (var contributor in Options.MenuContributors)
                    {
                        await contributor.ConfigureMenuAsync(context);
                    }

                    await CheckPermissionsAsync(scope.ServiceProvider, menu);
                }
            }

            NormalizeMenu(menu);

            return menu;
        }

        protected virtual async Task CheckPermissionsAsync(IServiceProvider serviceProvider, IHasMenuItems menuWithItems)
        {
            var allMenuItems = new List<ApplicationMenuItem>();
            GetAllMenuItems(menuWithItems, allMenuItems);

            foreach (var item in allMenuItems)
            {
                if (!item.RequiredPermissionName.IsNullOrWhiteSpace())
                {
                    item.RequirePermissions(item.RequiredPermissionName);
                }
            }

            var checkPermissionsMenuItems = allMenuItems.Where(x => x.StateCheckers.Any()).ToArray();

            if (checkPermissionsMenuItems.Any())
            {
                var toBeDeleted = new HashSet<ApplicationMenuItem>();
                var result =  await SimpleStateCheckerManager.IsEnabledAsync(checkPermissionsMenuItems);
                foreach (var menu in checkPermissionsMenuItems)
                {
                    if (!result[menu])
                    {
                        toBeDeleted.Add(menu);
                    }
                }

                RemoveMenus(menuWithItems, toBeDeleted);
            }
        }

        protected virtual void GetAllMenuItems(IHasMenuItems menuWithItems, List<ApplicationMenuItem> output)
        {
            foreach (var item in menuWithItems.Items)
            {
                output.Add(item);
                GetAllMenuItems(item, output);
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
