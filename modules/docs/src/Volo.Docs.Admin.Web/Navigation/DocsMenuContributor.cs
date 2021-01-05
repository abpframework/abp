using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Docs.Localization;

namespace Volo.Docs.Admin.Navigation
{
    public class DocsMenuContributor : IMenuContributor
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
            var administrationMenu = context.Menu.GetAdministration();

            var l = context.GetLocalizer<DocsResource>();

            var rootMenuItem = new ApplicationMenuItem(DocsMenuNames.GroupName, l["Menu:Documents"], icon: "fa fa-book");

            administrationMenu.AddItem(rootMenuItem);

            if (await context.IsGrantedAsync(DocsAdminPermissions.Projects.Default))
            {
                rootMenuItem.AddItem(new ApplicationMenuItem(DocsMenuNames.Projects, l["Menu:ProjectManagement"], "~/Docs/Admin/Projects"));
            }

            if (await context.IsGrantedAsync(DocsAdminPermissions.Documents.Default))
            {
                rootMenuItem.AddItem(new ApplicationMenuItem(DocsMenuNames.Documents, l["Menu:DocumentManagement"], "~/Docs/Admin/Documents"));
            }
        }
    }
}
