using System.Threading.Tasks;

namespace Volo.Abp.Ui.Navigation
{
    public interface IMenuManager
    {
        Task<ApplicationMenu> GetAsync(string name);
    }
}
