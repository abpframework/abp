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

                var menuContributorBases = Options.MenuContributors.OfType<MenuContributorBase>().ToList();
                var preCheckPermissions = menuContributorBases.SelectMany(x => x.PreCheckPermissions).ToArray();

                if (preCheckPermissions.Any())
                {
                    var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();
                    var grantResult = await permissionChecker.IsGrantedAsync(preCheckPermissions);

                    foreach (var menuContributorBase in menuContributorBases)
                    {
                        foreach (var result in grantResult.Result.Where(x => menuContributorBase.PreCheckPermissions.Contains(x.Key)))
                        {
                            if (result.Value == PermissionGrantResult.Granted)
                            {
                                menuContributorBase.AddGrantedPermission(result.Key);
                            }
                        }
                    }
                }

                foreach (var contributor in Options.MenuContributors)
                {
                    await contributor.ConfigureMenuAsync(context);
                }
            }

            NormalizeMenu(menu);

            return menu;
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
