﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account.Web.Modules.Account.Components.Toolbar.UserLoginLink
{
    public class LoginLinkViewComponent : AbpViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/Toolbar/LoginLink/Default.cshtml");
        }
    }
}
