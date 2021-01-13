using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.UI.Theming.Toolbars
{
    public interface IToolbarManager
    {
        Task<Toolbar> GetAsync(string name);
    }
}
