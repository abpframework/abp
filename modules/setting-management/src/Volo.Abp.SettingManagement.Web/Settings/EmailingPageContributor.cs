using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup;

namespace Volo.Abp.SettingManagement.Web.Settings;

public class EmailingPageContributor : SettingPageContributorBase
{
    public EmailingPageContributor()
    {
        RequiredFeatures(SettingManagementFeatures.Enable);
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
        return Task.CompletedTask;
    }
}
