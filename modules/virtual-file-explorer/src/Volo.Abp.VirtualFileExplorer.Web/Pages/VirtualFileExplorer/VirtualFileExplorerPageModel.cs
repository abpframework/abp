using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.VirtualFileExplorer.Web.Localization;

namespace Volo.Abp.VirtualFileExplorer.Web.Pages.VirtualFileExplorer
{
    public abstract class VirtualFileExplorerPageModel : AbpPageModel
    {
        protected VirtualFileExplorerPageModel()
        {
            LocalizationResourceType = typeof(VirtualFileExplorerResource);
            ObjectMapperContext = typeof(AbpVirtualFileExplorerWebModule);
        }
    }
}
