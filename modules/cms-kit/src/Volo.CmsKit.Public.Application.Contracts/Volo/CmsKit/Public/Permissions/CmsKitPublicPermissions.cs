using Volo.Abp.Reflection;

namespace Volo.CmsKit.Public.Permissions
{
    public class CmsKitPublicPermissions
    {
        public const string GroupName = "CmsKit.Public";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CmsKitPublicPermissions));
        }
    }
}
