using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Themes.Basic
{
    public partial class LoginDisplay
    {
        [Inject] protected IMenuManager MenuManager { get; set; }

        protected ApplicationMenu Menu { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Menu = await MenuManager.GetAsync(StandardMenus.User);
        }
    }
}