using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.PageToolbars
{
    public interface IPageToolbarContributor
    {
        Task ContributeAsync(PageToolbarContributionContext context);
    }
}
