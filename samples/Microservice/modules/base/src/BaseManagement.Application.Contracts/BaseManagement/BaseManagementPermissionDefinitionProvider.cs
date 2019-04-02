using BaseManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BaseManagement
{
    public class BaseManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var baseManagementGroup = context.AddGroup(BaseManagementPermissions.GroupName, L("Permission:BaseManagement"));

            var baseTypes = baseManagementGroup.AddPermission(BaseManagementPermissions.BaseTypes.Default, L("Permission:BaseTypes"));
            baseTypes.AddChild(BaseManagementPermissions.BaseTypes.Update, L("Permission:BaseTypes:Edit"));
            baseTypes.AddChild(BaseManagementPermissions.BaseTypes.Delete, L("Permission:BaseTypes:Delete"));
            baseTypes.AddChild(BaseManagementPermissions.BaseTypes.Create, L("Permission:BaseTypes:Create"));

            var baseItems = baseManagementGroup.AddPermission(BaseManagementPermissions.BaseItems.Default, L("Permission:BaseItems"));
            baseItems.AddChild(BaseManagementPermissions.BaseItems.Update, L("Permission:BaseItems:Edit"));
            baseItems.AddChild(BaseManagementPermissions.BaseItems.Delete, L("Permission:BaseItems:Delete"));
            baseItems.AddChild(BaseManagementPermissions.BaseItems.Create, L("Permission:BaseItems:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BaseManagementResource>(name);
        }
    }
}