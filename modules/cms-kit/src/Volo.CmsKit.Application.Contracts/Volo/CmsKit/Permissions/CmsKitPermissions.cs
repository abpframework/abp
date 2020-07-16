using Volo.Abp.Reflection;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitPermissions
    {
        public const string GroupName = "CmsKit";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CmsKitPermissions));
        }
    }
}