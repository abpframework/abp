using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public abstract class AccountPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<AccountResource> L { get; set; }
    }
}
