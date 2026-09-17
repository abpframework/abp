using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileExplorer.Localization;

namespace Volo.Abp.VirtualFileExplorer;

public class AbpVirtualFileExplorerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var virtualFileExplorer = context.AddGroup(VirtualFileExplorerPermissions.GroupName, L("Permission:AbpVirtualFileExplorer"));
        virtualFileExplorer.AddPermission(VirtualFileExplorerPermissions.View, L("Permission:AbpVirtualFileExplorer:View"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VirtualFileExplorerResource>(name);
    }
}
