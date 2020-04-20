using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionTestPage : AbpPageModel
    {
        public void OnGetUserFriendlyException1()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }
        
        public IActionResult OnGetUserFriendlyException2()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }
    }
}