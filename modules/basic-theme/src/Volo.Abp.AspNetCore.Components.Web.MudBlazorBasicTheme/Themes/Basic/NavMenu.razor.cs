using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme.Themes.Basic;

public partial class NavMenu
{
    protected ApplicationMenu MenuItems { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        MenuItems = await MenuManager.GetMainMenuAsync();
    }
}
