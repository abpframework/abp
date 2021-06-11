using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars
{
    public interface IToolbarManager
    {
        Task<Toolbar> GetAsync(string name);
    }
}
