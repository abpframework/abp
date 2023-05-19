using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

public interface IPageToolbarContributor
{
    Task ContributeAsync(PageToolbarContributionContext context);
}
