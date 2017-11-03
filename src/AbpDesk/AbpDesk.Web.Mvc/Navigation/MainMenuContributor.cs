using System.Threading.Tasks;
using Volo.Abp.Ui.Navigation;

namespace AbpDesk.Web.Mvc.Navigation
{
    public class MainMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            context.Menu.DisplayName = "Main Menu";

            context.Menu.Items.Insert(0, new ApplicationMenuItem("Home", "Home", url: "/"));

            context.Menu
                .AddItem(
                    new ApplicationMenuItem("TicketManagement", "Ticket Management")
                        .AddItem(
                            new ApplicationMenuItem("TicketManagement.Tickets", "Tickets", url: "/App/Tickets")
                        )
                );
            
            //Disabled blog module. This should be inside the module!
            //.AddItem(
            //    new ApplicationMenuItem("Blog", "Blog", url: "/Blog/Posts")
            //);

            return Task.CompletedTask;
        }
    }
}
