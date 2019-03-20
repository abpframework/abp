using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.Ui.Navigation.Localization.Resource;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.Ui.Navigation
{
    public class DefaultMenuContributor : IMenuContributor
    {
        public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            Configure(context);
            return Task.CompletedTask;
        }

        protected virtual void Configure(MenuConfigurationContext context)
        {
            var l = context.ServiceProvider
                .GetRequiredService<IStringLocalizer<AbpUiNavigationResource>>();

            if (context.Menu.Name == StandardMenus.Main)
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        DefaultMenuNames.Application.Main.Administration,
                        l["Menu:Administration"],
                        icon: "fa fa-wrench"
                    )
                );
            }
        }
    }
}
