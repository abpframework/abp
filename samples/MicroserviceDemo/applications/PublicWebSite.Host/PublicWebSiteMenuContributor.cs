using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace PublicWebSite.Host
{
    public class PublicWebSiteMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            //TODO: Localize menu items
            context.Menu.AddItem(new ApplicationMenuItem("Blog", "Blog", "/blog/abp"));

            return Task.CompletedTask;
        }
    }
}
