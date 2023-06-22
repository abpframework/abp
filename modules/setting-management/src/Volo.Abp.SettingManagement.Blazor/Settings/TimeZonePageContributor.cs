using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.TimeZoneSettingGroup;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.Timing;

namespace Volo.Abp.SettingManagement.Blazor.Settings;

public class TimeZonePageContributor : ISettingComponentContributor
{
    public async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        await CheckFeatureAsync(context);
        
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingManagementResource>>();
        if (context.ServiceProvider.GetRequiredService<IClock>().SupportsMultipleTimezone)
        {
            context.Groups.Add(
                new SettingComponentGroup(
                    "Volo.Abp.TimeZone",
                    l["Menu:TimeZone"],
                    typeof(TimeZoneSettingGroupViewComponent)
                )
            );
        }
    }

    public Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
    {
        return Task.FromResult(true);
    }
    
    private async Task<bool> CheckFeatureAsync(SettingComponentCreationContext context)
    {
        var featureCheck = context.ServiceProvider.GetRequiredService<IFeatureChecker>();

        return await featureCheck.IsEnabledAsync(SettingManagementFeatures.EnableTimeZone);
    }
}