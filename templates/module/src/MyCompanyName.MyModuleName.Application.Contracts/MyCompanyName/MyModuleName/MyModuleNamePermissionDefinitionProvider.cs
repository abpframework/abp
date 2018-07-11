using MyCompanyName.MyModuleName.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MyCompanyName.MyModuleName
{
    public class MyModuleNamePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //var moduleGroup = context.AddGroup(MyModuleNamePermissions.GroupName, L("Permission:MyModuleName"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MyModuleNameResource>(name);
        }
    }
}