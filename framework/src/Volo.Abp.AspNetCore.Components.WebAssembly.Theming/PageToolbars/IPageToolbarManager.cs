using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.PageToolbars
{
    public interface IPageToolbarManager
    {
        Task<PageToolbarItem[]> GetItemsAsync(string pageName);
    }
}
