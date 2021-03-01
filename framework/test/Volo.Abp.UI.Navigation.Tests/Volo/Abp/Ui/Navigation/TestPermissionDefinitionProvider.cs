using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.UI.Navigation
{
    public class TestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup("TestGroup");

            group.AddPermission("Dashboard");

            group.AddPermission("Administration");
            group.AddPermission("Administration.UserManagement");
            group.AddPermission("Administration.RoleManagement");

            group.AddPermission("Administration.DashboardSettings");

            group.AddPermission("Administration.SubMenu1");
            group.AddPermission("Administration.SubMenu1.1");
            group.AddPermission("Administration.SubMenu1.2");
        }
    }
}
