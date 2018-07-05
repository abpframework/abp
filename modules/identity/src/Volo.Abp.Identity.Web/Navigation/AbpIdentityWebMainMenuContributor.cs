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
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<IdentityResource>>();

            var identityMenuItem = new ApplicationMenuItem("Identity", l["Menu:IdentityManagement"], icon: "fa fa-id-card");
            context.Menu.AddItem(identityMenuItem);

            if (await authorizationService.IsGrantedAsync(IdentityPermissions.Roles.Default))
            {
                identityMenuItem.AddItem(new ApplicationMenuItem("Roles", l["Roles"], url: "/Identity/Roles"));
            }

            if (await authorizationService.IsGrantedAsync(IdentityPermissions.Users.Default))
            {
                identityMenuItem.AddItem(new ApplicationMenuItem("Users", l["Users"], url: "/Identity/Users"));
            }
        }
    }
}