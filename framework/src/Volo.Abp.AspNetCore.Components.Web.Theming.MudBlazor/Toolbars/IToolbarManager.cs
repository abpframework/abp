using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Toolbars;

public interface IToolbarManager
{
    Task<Toolbar> GetAsync(string name);
}
