using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.PageToolbars
{
    public abstract class PageToolbarContributor : IPageToolbarContributor
    {
        public abstract Task ContributeAsync(PageToolbarContributionContext context);
    }
}
