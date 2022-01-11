using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Authorization;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionTestPage : AbpPageModel
    {
        public void OnGetUserFriendlyException_void()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }

        public Task OnGetUserFriendlyException_Task()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }

        public IActionResult OnGetUserFriendlyException_ActionResult()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }

        public JsonResult OnGetUserFriendlyException_JsonResult()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }

        public Task<JsonResult> OnGetUserFriendlyException_Task_JsonResult()
        {
            throw new UserFriendlyException("This is a sample exception!");
        }

        public Task<JsonResult> OnGetAbpAuthorizationException()
        {
            throw new AbpAuthorizationException("This is a sample exception!");
        }
    }
}
