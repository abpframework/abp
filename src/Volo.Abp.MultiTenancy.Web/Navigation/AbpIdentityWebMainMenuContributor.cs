using System.Threading.Tasks;
using Volo.Abp.Ui.Navigation;

namespace Volo.Abp.MultiTenancy.Web.Navigation
{
    public class AbpMultiTenancyWebMainMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            context.Menu
                .AddItem(
                    new ApplicationMenuItem("MultiTenancy", "MultiTenancy")
                        .AddItem(new ApplicationMenuItem("Tenants", "Tenants", url: "/MultiTenancy/Tenants"))
                );

            return Task.CompletedTask;
        }
    }
}