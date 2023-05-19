
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions;

public class CmsKitPublicPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var cmsGroup = context.GetGroupOrNull(CmsKitPublicPermissions.GroupName) ?? context.AddGroup(CmsKitPublicPermissions.GroupName, L("Permission:CmsKitPublic"));

        var contentGroup = cmsGroup.AddPermission(CmsKitPublicPermissions.Comments.Default, L("Permission:Comments"))
            .RequireGlobalFeatures(typeof(CommentsFeature));
        contentGroup.AddChild(CmsKitPublicPermissions.Comments.DeleteAll, L("Permission:Comments.DeleteAll"))
            .RequireGlobalFeatures(typeof(CommentsFeature));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsKitResource>(name);
    }
}
