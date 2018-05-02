using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Views.Shared.Components.Theme.MainNavbar.Toolbar.Items.UserMenu;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarManager : IToolbarManager, ITransientDependency
    {
        public async Task<Toolbar> GetAsync(string name)
        {
            return new Toolbar(name)
            {
                Items =
                {
                    new ToolbarItem(typeof(UserMenuViewComponent))
                }
            };
        }
    }
}