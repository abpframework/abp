using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    [Authorize]
    public class AuthTestController : AbpController
    {
        public static Guid FakeUserId { get; } = new Guid();

        [AllowAnonymous]
        public ActionResult AnonymousTest()
        {
            return Content("OK");
        }

        public ActionResult SimpleAuthorizationTest()
        {
            CurrentUser.Id.ShouldBe(FakeUserId);
            return Content("OK");
        }

        [Authorize("MyClaimTestPolicy")]
        public ActionResult CustomPolicyTest()
        {
            return Content("OK");
        }

        //[Authorize("TestPermission")]
        //public ActionResult PermissionTest()
        //{
        //    return Content("OK");
        //}
    }
}
