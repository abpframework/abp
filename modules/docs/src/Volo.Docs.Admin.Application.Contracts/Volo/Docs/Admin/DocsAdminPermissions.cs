using Volo.Abp.Reflection;

namespace Volo.Docs.Admin
{
    public class DocsAdminPermissions
    {
        public const string GroupName = "Docs.Admin";

        public static class Projects
        {
            public const string Default = GroupName + ".Projects";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Documents
        {
            public const string Default = GroupName + ".Documents";
        }


        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DocsAdminPermissions));
        }
    }
}
