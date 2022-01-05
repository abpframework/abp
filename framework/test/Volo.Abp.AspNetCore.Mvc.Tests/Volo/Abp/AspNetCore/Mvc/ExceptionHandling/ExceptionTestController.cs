using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Authorization;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling;

[Route("api/exception-test")]
public class ExceptionTestController : AbpController
{
    [HttpGet]
    [Route("UserFriendlyException1")]
    public void UserFriendlyException1()
    {
        throw new UserFriendlyException("This is a sample exception!");
    }

    [HttpGet]
    [Route("UserFriendlyException2")]
    public ActionResult UserFriendlyException2()
    {
        throw new UserFriendlyException("This is a sample exception!");
    }

    [HttpGet]
    [Route("AbpAuthorizationException")]
    public void AbpAuthorizationException()
    {
        throw new AbpAuthorizationException("This is a sample exception!");
    }
}
