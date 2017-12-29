using System.Threading.Tasks;
using Volo.Abp.Ui.Navigation;

namespace Volo.Abp.Identity.Web.Navigation
{
    public class AbpIdentityWebMainMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            context.Menu
                .AddItem(
                    new ApplicationMenuItem("Identity", "Identity")
                        .AddItem(new ApplicationMenuItem("Users", "Users", url: "/Identity/Users"))
                        .AddItem(new ApplicationMenuItem("Roles", "Roles", url: "/Identity/Roles"))

                );

            return Task.CompletedTask;
        }
    }
}