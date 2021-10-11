using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Admin
{
    public class BloggingAdminMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<BloggingResource>();

            var managementRootMenuItem = new ApplicationMenuItem("BlogManagement", l["Menu:BlogManagement"]).RequirePermissions(BloggingPermissions.Blogs.Management);

            managementRootMenuItem.AddItem(new ApplicationMenuItem("BlogManagement.Blogs", l["Menu:Blogs"], "~/Blogging/Admin/Blogs").RequirePermissions(BloggingPermissions.Blogs.Management));

            context.Menu.GetAdministration().AddItem(managementRootMenuItem);

            return Task.CompletedTask;
        }
    }
}
