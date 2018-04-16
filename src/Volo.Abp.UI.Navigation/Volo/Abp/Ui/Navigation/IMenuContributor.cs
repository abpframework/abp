using System.Threading.Tasks;

namespace Volo.Abp.UI.Navigation
{
    public interface IMenuContributor
    {
        Task ConfigureMenuAsync(MenuConfigurationContext context);
    }
}