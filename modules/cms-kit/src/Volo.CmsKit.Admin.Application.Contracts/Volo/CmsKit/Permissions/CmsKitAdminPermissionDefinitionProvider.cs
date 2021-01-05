using Volo.Abp.Authorization.Permissions;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var cmsGroup = context.GetGroupOrNull(CmsKitAdminPermissions.GroupName) ?? context.AddGroup(CmsKitAdminPermissions.GroupName, L("Permission:CmsKit"));

            if (GlobalFeatureManager.Instance.IsEnabled<TagsFeature>())
            {
                var tagGroup = cmsGroup.AddPermission(CmsKitAdminPermissions.Tags.Default, L("Permission:TagManagement"));
                tagGroup.AddChild(CmsKitAdminPermissions.Tags.Create, L("Permission:TagManagement.Create"));
                tagGroup.AddChild(CmsKitAdminPermissions.Tags.Update, L("Permission:TagManagement.Update"));
                tagGroup.AddChild(CmsKitAdminPermissions.Tags.Delete, L("Permission:TagManagement.Delete"));
            }
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsKitResource>(name);
        }
    }
}
