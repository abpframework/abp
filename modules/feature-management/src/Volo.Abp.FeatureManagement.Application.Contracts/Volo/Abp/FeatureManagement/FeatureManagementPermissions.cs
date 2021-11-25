using Volo.Abp.Reflection;

namespace Volo.Abp.FeatureManagement;

public class FeatureManagementPermissions
{
    public const string GroupName = "FeatureManagement";

    public const string ManageHostFeatures = GroupName + ".ManageHostFeatures";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FeatureManagementPermissions));
    }
}
