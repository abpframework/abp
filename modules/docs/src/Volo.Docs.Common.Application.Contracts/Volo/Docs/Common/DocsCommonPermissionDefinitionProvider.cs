using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Docs.Localization;

namespace Volo.Docs.Common
{
    public class DocsCommonPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(DocsCommonPermissions.GroupName, L("Permission:DocumentManagement.Common"));

            group.AddPermission(DocsCommonPermissions.Projects.PdfDownload, L("Permission:PdfDownload"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DocsResource>(name);
        }
    }
}
