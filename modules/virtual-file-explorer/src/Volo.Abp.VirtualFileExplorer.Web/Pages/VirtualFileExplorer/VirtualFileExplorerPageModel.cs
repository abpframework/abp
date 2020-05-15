using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.VirtualFileExplorer.Localization;

namespace Volo.Abp.VirtualFileExplorer.Web.Pages.VirtualFileExplorer
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class VirtualFileExplorerPageModel : AbpPageModel
    {
        protected VirtualFileExplorerPageModel()
        {
            LocalizationResourceType = typeof(VirtualFileExplorerResource);
            ObjectMapperContext = typeof(AbpVirtualFileExplorerWebModule);
        }
    }
}
