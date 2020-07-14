using Volo.Abp.Reflection;

namespace Volo.CmsKit.Public.Permissions
{
    public class PublicPermissions
    {
        public const string GroupName = "Public";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(PublicPermissions));
        }
    }
}