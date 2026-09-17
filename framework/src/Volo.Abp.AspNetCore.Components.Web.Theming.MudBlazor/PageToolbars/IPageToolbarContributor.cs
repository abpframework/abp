using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;

public interface IPageToolbarContributor
{
    Task ContributeAsync(PageToolbarContributionContext context);
}
