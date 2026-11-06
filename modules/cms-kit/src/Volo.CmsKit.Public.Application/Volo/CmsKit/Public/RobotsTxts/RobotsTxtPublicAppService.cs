using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.RobotsTxts;

namespace Volo.CmsKit.Public.RobotsTxts;

[RequiresFeature(CmsKitFeatures.RobotsTxtEnable)]
[RequiresGlobalFeature(typeof(RobotsTxtFeature))]
public class RobotsTxtPublicAppService : CmsKitPublicAppServiceBase, IRobotsTxtPublicAppService
{
    protected RobotsTxtManager RobotsTxtManager { get; }

    public RobotsTxtPublicAppService(RobotsTxtManager robotsTxtManager)
    {
        RobotsTxtManager = robotsTxtManager;
    }

    public virtual async Task<RobotsTxtPublicDto> GetAsync()
    {
        return new RobotsTxtPublicDto
        {
            Content = await RobotsTxtManager.GetEffectiveContentOrNullAsync()
        };
    }
}
