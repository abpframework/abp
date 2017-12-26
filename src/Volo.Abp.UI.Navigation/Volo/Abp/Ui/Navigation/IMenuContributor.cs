using System.Threading.Tasks;

namespace Volo.Abp.Ui.Navigation
{
    public interface IMenuContributor
    {
        Task ConfigureMenuAsync(MenuConfigurationContext context);
    }
}