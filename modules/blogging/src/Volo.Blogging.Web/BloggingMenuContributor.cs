using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    public class BloggingMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenu(context);
            }
        }

        private async Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<BloggingResource>>();

            if (await authorizationService.IsGrantedAsync(BloggingPermissions.Blogs.Default))
            {
                var rootMenuItem = new ApplicationMenuItem("Blogs", l["Menu:Blogs"], "/Blog");

                context.Menu.AddItem(rootMenuItem);

                var managementRootMenuItem = new ApplicationMenuItem("BlogManagement", l["Menu:BlogManagement"]);
                managementRootMenuItem.AddItem(new ApplicationMenuItem("Blogs", l["Menu:Blogs"], "/Admin/Blogs"));

                context.Menu.AddItem(managementRootMenuItem);
            }


        }
    }
}
