using DashboardDemo.Localization.DashboardDemo;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace DashboardDemo.Permissions
{
    public class DashboardDemoPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(DashboardDemoPermissions.GroupName);

            //Define your own permissions here. Examaple:
            //myGroup.AddPermission(DashboardDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DashboardDemoResource>(name);
        }
    }
}
