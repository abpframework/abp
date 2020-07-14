using Volo.Abp.Reflection;

namespace Volo.CmsKit.Admin.Permissions
{
    public class AdminPermissions
    {
        public const string GroupName = "Admin";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(AdminPermissions));
        }
    }
}