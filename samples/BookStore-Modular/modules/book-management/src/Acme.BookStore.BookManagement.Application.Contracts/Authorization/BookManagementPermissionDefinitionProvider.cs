using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.BookStore.BookManagement.Authorization
{
    public class BookManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //var moduleGroup = context.AddGroup(BookManagementPermissions.GroupName, L("Permission:BookManagement"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BookManagementResource>(name);
        }
    }
}