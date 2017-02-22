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

            context.Menu
                .AddItem(
                    new ApplicationMenuItem("Home", "Home", url: "/")
                )
                .AddItem(
                    new ApplicationMenuItem("TicketManagement", "Ticket Management")
                        .AddItem(
                            new ApplicationMenuItem("TicketManagement.Tickets", "Tickets", url: "/Tickets")
                        )
                );

            return Task.CompletedTask;
        }
    }
}
