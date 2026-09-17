using Volo.Abp.Reflection;

namespace Volo.Abp.VirtualFileExplorer;

public static class VirtualFileExplorerPermissions
{
    public const string GroupName = "AbpVirtualFileExplorer";

    public const string View = GroupName + ".View";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(VirtualFileExplorerPermissions));
    }
}
