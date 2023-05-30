using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup;
using Volo.Abp.Timing;

namespace Volo.Abp.SettingManagement.Web.Settings;

public class EmailingPageContributor : SettingPageContributorBase
{
    public EmailingPageContributor()
    {
        RequiredTenantSideFeatures(SettingManagementFeatures.Enable);
        RequiredTenantSideFeatures(SettingManagementFeatures.AllowChangingEmailSettings);
        RequiredPermissions(SettingManagementPermissions.Emailing);
    }
    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingManagementResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.EmailSetting",
                l["Menu:Emailing"],
                typeof(EmailSettingGroupViewComponent)
            )
        );

        if (!context.ServiceProvider.GetRequiredService<IClock>().SupportsMultipleTimezone)
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
