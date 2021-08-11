using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MyCompanyName.MyProjectName.Permissions
{
    public class MyProjectNamePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(MyProjectNamePermissions.GroupName);

            //Define your own permissions here. Example:
            myGroup.AddPermission("permission1", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission2", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission3", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission4", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission5", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission6", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission7", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission8", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission9", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission10", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission11", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission12", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission13", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission14", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission15", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission16", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission17", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission18", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission19", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission20", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission21", L("Permission:MyPermission1"));
            myGroup.AddPermission("permission22", L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MyProjectNameResource>(name);
        }
    }
}
