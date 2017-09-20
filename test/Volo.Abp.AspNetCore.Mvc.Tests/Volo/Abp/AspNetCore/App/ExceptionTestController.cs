using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Ui;

namespace Volo.Abp.AspNetCore.App
{
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
    }
}
