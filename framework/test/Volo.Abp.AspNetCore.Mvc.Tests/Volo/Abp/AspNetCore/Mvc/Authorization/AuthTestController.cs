using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Volo.Abp.AspNetCore.Mvc.Authorization
{
    [Authorize]
    public class AuthTestController : AbpController
    {
        public static Guid FakeUserId { get; } = Guid.NewGuid();

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
            CurrentUser.Id.ShouldBe(FakeUserId);
            var claim = CurrentUser.FindClaim("MyCustomClaimType");
            claim.ShouldNotBeNull();
            claim.Value.ShouldBe("42");
            return Content("OK");
        }

        [Authorize("TestPermission1")]
        public ActionResult PermissionTest()
        {
            CurrentUser.Id.ShouldBe(FakeUserId);
            return Content("OK");
        }

        [Authorize("TestPermission1_And_TestPermission2")]
        public ActionResult Custom_And_PolicyTest()
        {
            CurrentUser.Id.ShouldBe(FakeUserId);
            return Content("OK");
        }

        [Authorize("TestPermission1_Or_TestPermission2")]
        public ActionResult Custom_Or_PolicyTest()
        {
            CurrentUser.Id.ShouldBe(FakeUserId);
            return Content("OK");
        }
    }
}
