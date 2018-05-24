using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.RazorPages
{
    public abstract class AbpPage : Page
    {
        [RazorInject]
        protected ICurrentUser CurrentUser { get; set; }
    }
}
