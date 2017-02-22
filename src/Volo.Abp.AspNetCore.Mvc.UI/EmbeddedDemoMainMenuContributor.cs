using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class EmbeddedDemoMainMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            context.Menu
                .AddItem(
                    new ApplicationMenuItem("EmbeddedDemo", "EmbeddedDemo", url: "/AbpEmbeddedDemo")
                );

            return Task.CompletedTask;
        }
    }
}