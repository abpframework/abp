using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.RobotsTxts;

[RequiresFeature(CmsKitFeatures.RobotsTxtEnable)]
[RequiresGlobalFeature(typeof(RobotsTxtFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitAdminRemoteServiceConsts.ModuleName)]
[Authorize(CmsKitAdminPermissions.RobotsTxt.Default)]
[Route("api/cms-kit-admin/robots-txt")]
public class RobotsTxtAdminController : CmsKitAdminController, IRobotsTxtAdminAppService
{
    protected IRobotsTxtAdminAppService RobotsTxtAdminAppService { get; }

    public RobotsTxtAdminController(IRobotsTxtAdminAppService robotsTxtAdminAppService)
    {
        RobotsTxtAdminAppService = robotsTxtAdminAppService;
    }

    [HttpGet]
    public virtual Task<RobotsTxtDto> GetAsync()
    {
        return RobotsTxtAdminAppService.GetAsync();
    }

    [HttpPut]
    public virtual Task UpdateAsync(UpdateRobotsTxtDto input)
    {
        return RobotsTxtAdminAppService.UpdateAsync(input);
    }

    [HttpDelete]
    public virtual Task ResetAsync()
    {
        return RobotsTxtAdminAppService.ResetAsync();
    }
}
