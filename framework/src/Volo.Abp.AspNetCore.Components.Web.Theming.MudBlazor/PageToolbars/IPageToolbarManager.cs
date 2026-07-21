using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

public interface IPageToolbarManager
{
    Task<PageToolbarItem[]> GetItemsAsync(PageToolbar toolbar);
}
