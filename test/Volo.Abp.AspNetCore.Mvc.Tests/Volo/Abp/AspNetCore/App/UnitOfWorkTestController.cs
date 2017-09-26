using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.App
{
    [Route("api/unitofwork-test")]
    public class UnitOfWorkTestController : AbpController
    {
        [HttpGet]
        [Route("ActionRequiresUow")]
        public ActionResult ActionRequiresUow()
        {
            CurrentUnitOfWork.ShouldNotBeNull();
            CurrentUnitOfWork.Options.IsTransactional.ShouldBeFalse();

            return Content("OK");
        }

        [HttpPost]
        [Route("ActionRequiresUowPost")]
        public ActionResult ActionRequiresUowPost()
        {
            CurrentUnitOfWork.ShouldNotBeNull();
            CurrentUnitOfWork.Options.IsTransactional.ShouldBeTrue();

            return Content("OK");
        }
    }
}
