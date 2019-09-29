using MyCompanyName.MyProjectName.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MyCompanyName.MyProjectName.Authorization
{
    public class MyProjectNamePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //var moduleGroup = context.AddGroup(MyProjectNamePermissions.GroupName, L("Permission:MyProjectName"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MyProjectNameResource>(name);
        }
    }
}