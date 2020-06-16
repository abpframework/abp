using Volo.Abp.Reflection;

namespace Volo.Blogging.Admin
{
    public class BloggingAdminPermissions
    {
        public const string GroupName = "Blogging.Admin";

        public static class Blogs
        {
            public const string Default = GroupName + ".Blog";
            public const string Management = Default + ".Management";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BloggingAdminPermissions));
        }
    }
}
