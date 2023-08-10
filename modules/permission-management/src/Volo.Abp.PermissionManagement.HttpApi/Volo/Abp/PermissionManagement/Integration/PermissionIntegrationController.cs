using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.PermissionManagement.Integration;

[RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionManagementRemoteServiceConsts.ModuleName)]
[ControllerName("PermissionIntegration")]
[Route("integration-api/permission-management/permissions")]
public class PermissionIntegrationController: AbpControllerBase, IPermissionIntegrationService
{
    protected IPermissionIntegrationService PermissionIntegrationService { get; }

    public PermissionIntegrationController(IPermissionIntegrationService permissionIntegrationService)
    {
        PermissionIntegrationService = permissionIntegrationService;
    }

    [HttpPost]
    [Route("is-granted")]
    public virtual Task<IsGrantedOutput> IsGrantedAsync(IsGrantedInput input)
    {
        return PermissionIntegrationService.IsGrantedAsync(input);
    }

    [HttpPost]
    [Route("is-granted/multiple")]
    public virtual Task<List<IsGrantedOutput>> IsGrantedAsync(List<IsGrantedInput> input)
    {
        return PermissionIntegrationService.IsGrantedAsync(input);
    }
}
