using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Shared.Components.Comments;

public class CommentSettingPageContributor : SettingPageContributorBase
{
    public CommentSettingPageContributor()
    {
        RequiredPermissions(CmsKitAdminPermissions.Comments.SettingManagement);
    }

    public override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<CmsKitResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.CmsKit",
                l["Settings:Menu:CmsKit"],
                typeof(CommentSettingViewComponent)
            )
        );

        return Task.CompletedTask;
    }
}