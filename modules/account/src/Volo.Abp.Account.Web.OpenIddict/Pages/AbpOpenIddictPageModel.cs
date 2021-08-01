using Volo.Abp.Account.Web.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.Account.Web.Pages
{
    public abstract class AbpOpenIddictPageModel : AbpPageModel
    {
        public AbpOpenIddictPageModel()
        {
            LocalizationResourceType = typeof(AbpOpenIddictWebResource);
        }
    }
}
