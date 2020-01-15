using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    [Route("api/unitofwork-test")]
    public class UnitOfWorkTestController : AbpController
    {
        private readonly TestUnitOfWorkConfig _testUnitOfWorkConfig;

        public UnitOfWorkTestController(TestUnitOfWorkConfig testUnitOfWorkConfig)
        {
            _testUnitOfWorkConfig = testUnitOfWorkConfig;
        }

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

        [HttpGet]
        [Route("HandledException")]
        [UnitOfWork(isTransactional: true)]
        public void HandledException()
        {
            CurrentUnitOfWork.ShouldNotBeNull();
            CurrentUnitOfWork.Options.IsTransactional.ShouldBeTrue();

            throw new UserFriendlyException("This is a sample exception!");
        }

        [HttpGet]
        [Route("ExceptionOnComplete")]
        public void ExceptionOnComplete()
        {
            CurrentUnitOfWork.ShouldNotBeNull();
            CurrentUnitOfWork.Options.IsTransactional.ShouldBeFalse();

            _testUnitOfWorkConfig.ThrowExceptionOnComplete = true;
        }
    }
}
