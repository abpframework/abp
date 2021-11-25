using System.Threading.Tasks;

namespace Volo.Abp.UI.Navigation;

public interface IMenuManager
{
    Task<ApplicationMenu> GetAsync(string name);

    Task<ApplicationMenu> GetMainMenuAsync();
}
