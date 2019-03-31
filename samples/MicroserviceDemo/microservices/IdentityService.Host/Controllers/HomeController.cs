using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Json;

namespace IdentityService.Host.Controllers
{
    public class HomeController : AbpController
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IPermissionChecker _permissionChecker;

        public HomeController(IJsonSerializer jsonSerializer, IPermissionChecker permissionChecker)
        {
            _jsonSerializer = jsonSerializer;
            _permissionChecker = permissionChecker;
        }

        public ActionResult Index()
        {
            return Redirect("/swagger");
        }

        public async Task<IActionResult> Get()
        {
            var newLine = Environment.NewLine + Environment.NewLine;

            return Content("Claims: " + User.Claims.Select(c => $"{c.Type} = {c.Value}").JoinAsString(" | ") + newLine +
                        "CurrentUser: " + _jsonSerializer.Serialize(CurrentUser) + newLine +
                        "access_token: " + await HttpContext.GetTokenAsync("access_token") + newLine +
                        "isGranted: AbpIdentity.Users: " +
                        await _permissionChecker.IsGrantedAsync("AbpIdentity.Users"));
        }
    }
}
