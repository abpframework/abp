using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Account.Blazor
{
    public class AbpAccountBlazorUserMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.User)
            {
                return Task.CompletedTask;
            }

            var accountResource = context.GetLocalizer<AccountResource>();

            context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountResource["MyAccount"], url: "account/manage-profile", icon: "fa fa-cog"));
            
            return Task.CompletedTask;
        }
    }
}