using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.TimeZoneSettingGroup;
using Volo.Abp.Timing;

namespace Volo.Abp.SettingManagement.Web.Settings;

public class TimeZonePageContributor : SettingPageContributorBase
{
    public TimeZonePageContributor()
    {
        RequiredPermissions(SettingManagementPermissions.TimeZone);
    }
    
    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingManagementResource>>();

        if (context.ServiceProvider.GetRequiredService<IClock>().SupportsMultipleTimezone)
        {
            context.Groups.Add(
                new SettingPageGroup(
                    "Volo.Abp.TimeZone",
                    l["Menu:TimeZone"],
                    typeof(TimeZoneSettingGroupViewComponent)
                )
            );
        }

        return Task.CompletedTask;
    }
}
