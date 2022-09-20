using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.FeatureManagement.Pages.FeatureManagement.Components.FeatureSettingGroup;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

namespace Volo.Abp.FeatureManagement.Settings;

public class FeatureSettingManagementPageContributor : SettingPageContributorBase
{
    public FeatureSettingManagementPageContributor()
    {
        RequiredPermissions(FeatureManagementPermissions.ManageHostFeatures);
    }

    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpFeatureManagementResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.FeatureManagement",
                l["Menu:FeatureManagement"],
                typeof(FeatureSettingGroupViewComponent)
            )
        );
        return Task.CompletedTask;
    }
}