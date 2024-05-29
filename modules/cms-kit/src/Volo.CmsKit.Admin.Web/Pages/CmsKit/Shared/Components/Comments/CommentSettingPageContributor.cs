using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.CmsKit.Localization;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Shared.Components.Comments;

public class CommentSettingPageContributor : ISettingPageContributor
{
    public Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<CmsKitResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.MySettingGroup",
                l["Menu:CmsKitCommentOptions"],
                typeof(CommentSettingViewComponent),
                order: 1
            )
        );

        return Task.CompletedTask;
    }

    public async Task<bool> CheckPermissionsAsync(SettingPageCreationContext context)
    {
        var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

        return await authorizationService.IsGrantedAsync(CmsKitAdminPermissions.Comments.SettingManagement);
    }
}