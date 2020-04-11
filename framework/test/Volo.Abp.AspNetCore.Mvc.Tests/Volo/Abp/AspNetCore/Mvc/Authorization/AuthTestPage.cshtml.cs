using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    [Authorize]
    public class AuthTestPage : AbpPageModel
    {
        public static Guid FakeUserId { get; } = Guid.NewGuid();
        
        public ActionResult OnGet()
        {
            return Content("OK");
        }
    }
}