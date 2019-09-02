using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Identity.Web.Navigation
{
    public class AbpIdentityWebMainMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

            var hasRolePermission = await authorizationService.IsGrantedAsync(IdentityPermissions.Roles.Default);
            var hasUserPermission = await authorizationService.IsGrantedAsync(IdentityPermissions.Users.Default);

            if (hasRolePermission || hasUserPermission)
            {
                var administrationMenu = context.Menu.GetAdministration();


                var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();

                var identityMenuItem = new ApplicationMenuItem(IdentityMenuNames.GroupName, l["Menu:IdentityManagement"], icon: "fa fa-id-card-o");
                administrationMenu.AddItem(identityMenuItem);

                if (hasRolePermission)
                {
                    identityMenuItem.AddItem(new ApplicationMenuItem(IdentityMenuNames.Roles, l["Roles"], url: "/Identity/Roles"));
                }

                if (hasUserPermission)
                {
                    identityMenuItem.AddItem(new ApplicationMenuItem(IdentityMenuNames.Users, l["Users"], url: "/Identity/Users"));
                }
            }
        }
    }
}