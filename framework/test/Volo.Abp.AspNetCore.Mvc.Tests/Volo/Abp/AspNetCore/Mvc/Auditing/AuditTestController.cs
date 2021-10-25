using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;

namespace Volo.Abp.AspNetCore.Mvc.Auditing
{
    [Route("api/audit-test")]
    public class AuditTestController : AbpController
    {
        private readonly AbpAuditingOptions _options;

        public AuditTestController(IOptions<AbpAuditingOptions> options)
        {
            _options = options.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [Route("audit-success")]
        public IActionResult AuditSuccessForGetRequests()
        {
            return Ok();
        }

        [Route("audit-fail")]
        public IActionResult AuditFailForGetRequests()
        {
            throw new UserFriendlyException("Exception occurred!");
        }

        [Route("audit-fail-object")]
        public object AuditFailForGetRequestsReturningObject()
        {
            throw new UserFriendlyException("Exception occurred!");
        }

        [HttpGet]
        [Route("audit-activate-failed")]
        public IActionResult AuditActivateFailed([FromServices] AbpAuditingOptions options)
        {
            return Ok();
        }
    }
}
