using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.PageToolbars
{
    public interface IPageToolbarContributor
    {
        Task ContributeAsync(PageToolbarContributionContext context);
    }
}