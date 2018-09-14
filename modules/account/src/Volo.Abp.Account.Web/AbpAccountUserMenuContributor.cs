using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Account.Web
{
    public class AbpAccountUserMenuContributor : IMenuContributor
    {
        public AbpAccountUserMenuContributor()
        {

        }

        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.User)
            {
                return Task.CompletedTask;
            }

            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpUiResource>>();

            context.Menu.AddItem(new ApplicationMenuItem("Account.ChangePassword", l["ChangePassword"], icon: "fa fa-key", url: "#", elementId: "abp-account-change-password"));

            context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", l["Logout"], url: "/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000));

            return Task.CompletedTask;
        }
    }
}