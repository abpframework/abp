using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Identity.Web
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
                        .AddItem(new ApplicationMenuItem("Users", "Users", url: "/Users"))
                );

            return Task.CompletedTask;
        }
    }
}