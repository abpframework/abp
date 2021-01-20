using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
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

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<BloggingResource>();

            if (await context.IsGrantedAsync(BloggingPermissions.Blogs.Management))
            {
                var managementRootMenuItem = new ApplicationMenuItem("BlogManagement", l["Menu:BlogManagement"]);

                //TODO: Using the same permission. Reconsider.
                if (await context.IsGrantedAsync(BloggingPermissions.Blogs.Management))
                {
                    managementRootMenuItem.AddItem(new ApplicationMenuItem("BlogManagement.Blogs", l["Menu:Blogs"], "~/Blogging/Admin/Blogs"));
                }

                context.Menu.GetAdministration().AddItem(managementRootMenuItem);
            }
        }
    }
}
