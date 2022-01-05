using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.UI.Navigation;

public class MenuManager : IMenuManager, ITransientDependency
{
    protected AbpNavigationOptions Options { get; }
    protected IHybridServiceScopeFactory ServiceScopeFactory { get; }
    protected ISimpleStateCheckerManager<ApplicationMenuItem> SimpleStateCheckerManager { get; }
    public MenuManager(
        IOptions<AbpNavigationOptions> options,
        IHybridServiceScopeFactory serviceScopeFactory,
        ISimpleStateCheckerManager<ApplicationMenuItem> simpleStateCheckerManager)
    {
        Options = options.Value;
        ServiceScopeFactory = serviceScopeFactory;
        SimpleStateCheckerManager = simpleStateCheckerManager;
    }

    public Task<ApplicationMenu> GetAsync(string name)
    {
        return GetInternalAsync(name);
    }

    public Task<ApplicationMenu> GetMainMenuAsync()
    {
        return GetAsync(Options.MainMenuNames.ToArray());
    }

    protected virtual async Task<ApplicationMenu> GetAsync(params string[] menuNames)
    {
        if (menuNames.IsNullOrEmpty())
        {
            return new ApplicationMenu(StandardMenus.Main);
        }

        var menus = new List<ApplicationMenu>();

        foreach (var menuName in Options.MainMenuNames)
        {
            menus.Add(await GetInternalAsync(menuName));
        }

        return MergeMenus(menus);
    }

    protected virtual ApplicationMenu MergeMenus(List<ApplicationMenu> menus)
    {
        Check.NotNullOrEmpty(menus, nameof(menus));

        if (menus.Count == 1)
        {
            return menus[0];
        }

        var firstMenu = menus[0];

        for (int i = 1; i < menus.Count; i++)
        {
            var currentMenu = menus[i];
            foreach (var menuItem in currentMenu.Items)
            {
                firstMenu.AddItem(menuItem);
            }
        }

        return firstMenu;
    }

    protected virtual async Task<ApplicationMenu> GetInternalAsync(string name)
    {
        var menu = new ApplicationMenu(name);

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            using (RequirePermissionsSimpleBatchStateChecker<ApplicationMenuItem>.Use(new RequirePermissionsSimpleBatchStateChecker<ApplicationMenuItem>()))
            {
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
            var result = await SimpleStateCheckerManager.IsEnabledAsync(checkPermissionsMenuItems);
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
