using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Integration;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("UserIntegration")]
[Route("integration-api/identity/users")]
public class IdentityUserIntegrationController : AbpControllerBase, IIdentityUserIntegrationService
{
    protected IIdentityUserIntegrationService UserIntegrationService { get; }

    public IdentityUserIntegrationController(IIdentityUserIntegrationService userIntegrationService)
    {
        UserIntegrationService = userIntegrationService;
    }
    
    [HttpGet]
    [Route("{id}/role-names")]
    public virtual Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return UserIntegrationService.GetRoleNamesAsync(id);
    }
}