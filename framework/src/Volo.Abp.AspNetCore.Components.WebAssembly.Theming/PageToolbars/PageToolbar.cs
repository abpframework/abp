using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.PageToolbars
{
    public class PageToolbar
    {
        public PageToolbarContributorList Contributors { get; set; }

        public PageToolbar()
        {
            Contributors = new PageToolbarContributorList();
        }
    }
}
