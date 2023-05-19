using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Auditing;

[Route("integration-api/audit-test")]
[IntegrationService]
public class AuditIntegrationServiceTestController : AbpController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}