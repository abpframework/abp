using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var cmsGroup = context.GetGroupOrNull(CmsKitAdminPermissions.GroupName) ?? context.AddGroup(CmsKitAdminPermissions.GroupName, L("Permission:CmsKit"));

            cmsGroup
                .AddPermission(CmsKitAdminPermissions.Tags.Default, L("Permission:TagManagement"))
                    .AddChild(CmsKitAdminPermissions.Tags.Create, L("Permission:TagManagement.Create"))
                    .AddChild(CmsKitAdminPermissions.Tags.Update, L("Permission:TagManagement.Update"))
                    .AddChild(CmsKitAdminPermissions.Tags.Delete, L("Permission:TagManagement.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsKitResource>(name);
        }
    }
}
