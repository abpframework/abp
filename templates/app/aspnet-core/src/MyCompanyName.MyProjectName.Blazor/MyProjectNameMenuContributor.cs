using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace MyCompanyName.MyProjectName.Blazor
{
    public class MyProjectNameMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            context.Menu.AddItem(new ApplicationMenuItem("Test", "Test", "/test"));

            return Task.CompletedTask;
        }
    }
}
