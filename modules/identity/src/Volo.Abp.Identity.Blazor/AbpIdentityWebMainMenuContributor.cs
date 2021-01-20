using System.Threading.Tasks;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Identity.Blazor
{
    public class AbpIdentityWebMainMenuContributor : IMenuContributor
    {
        public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            var hasRolePermission = await context.IsGrantedAsync(IdentityPermissions.Roles.Default);
            var hasUserPermission = await context.IsGrantedAsync(IdentityPermissions.Users.Default);

            if (hasRolePermission || hasUserPermission)
            {
                var administrationMenu = context.Menu.GetAdministration();

                var l = context.GetLocalizer<IdentityResource>();

                var identityMenuItem = new ApplicationMenuItem(IdentityMenuNames.GroupName, l["Menu:IdentityManagement"], icon: "far fa-id-card");
                administrationMenu.AddItem(identityMenuItem);

                if (hasRolePermission)
                {
                    identityMenuItem.AddItem(new ApplicationMenuItem(IdentityMenuNames.Roles, l["Roles"], url: "identity/roles"));
                }

                if (hasUserPermission)
                {
                    identityMenuItem.AddItem(new ApplicationMenuItem(IdentityMenuNames.Users, l["Users"], url: "identity/users"));
                }
            }
        }
    }
}
