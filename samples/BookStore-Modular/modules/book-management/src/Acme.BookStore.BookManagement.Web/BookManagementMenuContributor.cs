using System.Threading.Tasks;
using Acme.BookStore.BookManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;

namespace Acme.BookStore.BookManagement.Web
{
    public class BookManagementMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenu(context);
            }
        }

        private Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            var stringLocalizer = context.ServiceProvider.GetRequiredService<IStringLocalizer<BookManagementResource>>();

            context.Menu.AddItem(
                new ApplicationMenuItem("BookManagement", stringLocalizer["Menu:BookManagement"])
                    .AddItem(new ApplicationMenuItem("BookManagement.Books", stringLocalizer["Menu:Books"], url: "/BookManagement/Books"))
            );

            return Task.CompletedTask;
        }
    }
}