// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroserviceDemo.TenancyService.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class IdentityController : AbpController
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                currentUserId = CurrentUser.Id,
                currentUserEmail = CurrentUser.Email,
                currentUserRoles = CurrentUser.Roles,
                currentUserUserName = CurrentUser.UserName,
                claims = from c in User.Claims select new { c.Type, c.Value }
            });
        }
    }
}