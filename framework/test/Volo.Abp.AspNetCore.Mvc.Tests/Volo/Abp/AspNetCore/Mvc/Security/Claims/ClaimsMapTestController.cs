using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Volo.Abp.AspNetCore.Mvc.Security.Claims;

public class ClaimsMapTestController : AbpController
{
    public ActionResult ClaimsMapTest()
    {
        var serialNumber = CurrentUser.FindClaim(ClaimTypes.SerialNumber);
        serialNumber.ShouldNotBeNull();
        serialNumber.Value.ShouldBe("123456");

        var dateOfBirth = CurrentUser.FindClaim(ClaimTypes.DateOfBirth);
        dateOfBirth.ShouldNotBeNull();
        dateOfBirth.Value.ShouldBe("2020");

        return Content("OK");
    }
}
