using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public interface IToolbarManager
    {
        Task<Toolbar> GetAsync(string name);
    }
}
