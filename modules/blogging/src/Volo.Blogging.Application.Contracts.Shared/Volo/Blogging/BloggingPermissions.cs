using Volo.Abp.Reflection;

namespace Volo.Blogging
{
    public class BloggingPermissions
    {
        public const string GroupName = "Blogging";

        public static class Blogs
        {
            public const string Default = GroupName + ".Blog";
            public const string Management = Default + ".Management";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string ClearCache = Default + ".ClearCache";
        }

        public static class Posts
        {
            public const string Default = GroupName + ".Post";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Tags
        {
            public const string Default = GroupName + ".Tag";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Comments
        {
            public const string Default = GroupName + ".Comment";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BloggingPermissions));
        }
    }
}
