using Volo.Abp.Reflection;

namespace Volo.CmsKit.Permissions
{
    public class CmsKitAdminPermissions
    {
        public const string GroupName = "CmsKit.Admin";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CmsKitAdminPermissions));
        }
    }
}