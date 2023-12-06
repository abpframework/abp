using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
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

    [HttpGet]
    [Route("is-granted")]
    public virtual Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input)
    {
        return PermissionIntegrationService.IsGrantedAsync(input);
    }
}
