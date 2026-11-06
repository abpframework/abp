using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.RobotsTxts;

namespace Volo.CmsKit.Admin.RobotsTxts;

[RequiresFeature(CmsKitFeatures.RobotsTxtEnable)]
[RequiresGlobalFeature(typeof(RobotsTxtFeature))]
[Authorize(CmsKitAdminPermissions.RobotsTxt.Default)]
public class RobotsTxtAdminAppService : CmsKitAdminAppServiceBase, IRobotsTxtAdminAppService
{
    protected RobotsTxtManager RobotsTxtManager { get; }

    public RobotsTxtAdminAppService(RobotsTxtManager robotsTxtManager)
    {
        RobotsTxtManager = robotsTxtManager;
    }

    public virtual async Task<RobotsTxtDto> GetAsync()
    {
        var robotsTxt = await RobotsTxtManager.FindAsync();
        if (robotsTxt != null)
        {
            return new RobotsTxtDto { Content = robotsTxt.Content, Source = RobotsTxtSource.Custom };
        }

        if (CurrentTenant.IsAvailable)
        {
            var hostContent = await RobotsTxtManager.GetEffectiveContentOrNullAsync();
            if (hostContent != null)
            {
                return new RobotsTxtDto { Content = hostContent, Source = RobotsTxtSource.InheritedFromHost };
            }
        }

        return new RobotsTxtDto { Content = null, Source = RobotsTxtSource.Default };
    }

    public virtual async Task UpdateAsync(UpdateRobotsTxtDto input)
    {
        await RobotsTxtManager.SetAsync(input.Content);
    }

    public virtual async Task ResetAsync()
    {
        await RobotsTxtManager.ResetAsync();
    }
}
