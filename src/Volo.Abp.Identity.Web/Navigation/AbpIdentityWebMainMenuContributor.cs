using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Ui.Navigation;

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

            var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();

            //TODO: We should not add this if none of the children available. Alternatively, we can handle it on menu rendering (would be simpler).
            var identityMenuItem = new ApplicationMenuItem("Identity", "Identity");

            context.Menu.AddItem(identityMenuItem);

            if (await permissionChecker.IsGrantedAsync(IdentityPermissions.Roles.Default))
            {
                identityMenuItem.AddItem(new ApplicationMenuItem("Roles", "Roles", url: "/Identity/Roles"));
            }

            if (await permissionChecker.IsGrantedAsync(IdentityPermissions.Users.Default))
            {
                identityMenuItem.AddItem(new ApplicationMenuItem("Users", "Users", url: "/Identity/Users"));
            }
        }
    }
}