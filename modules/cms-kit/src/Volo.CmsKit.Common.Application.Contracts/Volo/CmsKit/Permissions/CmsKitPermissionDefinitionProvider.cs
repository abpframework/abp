using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(
                CmsKitPermissions.GroupName,
                L("Permission:CmsKit.Public")
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsKitResource>(name);
        }
    }
}
