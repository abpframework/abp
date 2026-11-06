using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.RobotsTxts;

[RequiresFeature(CmsKitFeatures.RobotsTxtEnable)]
[RequiresGlobalFeature(typeof(RobotsTxtFeature))]
[RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitPublicRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/robots-txt")]
public class RobotsTxtPublicController : CmsKitPublicControllerBase, IRobotsTxtPublicAppService
{
    protected IRobotsTxtPublicAppService RobotsTxtPublicAppService { get; }

    public RobotsTxtPublicController(IRobotsTxtPublicAppService robotsTxtPublicAppService)
    {
        RobotsTxtPublicAppService = robotsTxtPublicAppService;
    }

    [HttpGet]
    public virtual Task<RobotsTxtPublicDto> GetAsync()
    {
        return RobotsTxtPublicAppService.GetAsync();
    }
}
