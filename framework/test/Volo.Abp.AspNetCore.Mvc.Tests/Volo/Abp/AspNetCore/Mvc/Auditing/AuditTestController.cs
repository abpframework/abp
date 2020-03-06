using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;

namespace Volo.Abp.AspNetCore.Mvc.Auditing
{
    [Route("api/audit-test")]
    [Audited]
    public class AuditTestController : AbpController
    {
        private readonly AbpAuditingOptions _options;

        public AuditTestController(IOptions<AbpAuditingOptions> options)
        {
            _options = options.Value;
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
    }
}
