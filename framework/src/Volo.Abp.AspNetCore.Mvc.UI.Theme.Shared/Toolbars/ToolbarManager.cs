using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarManager : IToolbarManager, ITransientDependency
    {
        protected IThemeManager ThemeManager { get; }
        protected AbpToolbarOptions Options { get; }
        protected IServiceProvider ServiceProvider { get; }

        public ToolbarManager(
            IOptions<AbpToolbarOptions> options,
            IServiceProvider serviceProvider,
            IThemeManager themeManager)
        {
            ThemeManager = themeManager;
            ServiceProvider = serviceProvider;
            Options = options.Value;
        }

        public async Task<Toolbar> GetAsync(string name)
        {
            var toolbar = new Toolbar(name);

            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new ToolbarConfigurationContext(ThemeManager.CurrentTheme, toolbar, scope.ServiceProvider);

                foreach (var contributor in Options.Contributors)
                {
                    await contributor.ConfigureToolbarAsync(context);
                }

                await CheckPermissionsAsync(scope.ServiceProvider, toolbar);
            }

            return toolbar;
        }

        protected virtual async Task CheckPermissionsAsync(IServiceProvider serviceProvider, Toolbar toolbar)
        {
            var requiredPermissionItems = toolbar.Items.Where(x => !x.RequiredPermissionName.IsNullOrWhiteSpace()).ToList();

            if (requiredPermissionItems.Any())
            {
                var permissionChecker = serviceProvider.GetRequiredService<IPermissionChecker>();
                var grantResult = await permissionChecker.IsGrantedAsync(requiredPermissionItems.Select(x => x.RequiredPermissionName).Distinct().ToArray());

                var toBeDeleted = new HashSet<ToolbarItem>();
                foreach (var item in requiredPermissionItems)
                {
                    if (grantResult.Result[item.RequiredPermissionName!] != PermissionGrantResult.Granted)
                    {
                        toBeDeleted.Add(item);
                    }
                }

                toolbar.Items.RemoveAll(toBeDeleted.Contains);
            }
        }
    }
}
