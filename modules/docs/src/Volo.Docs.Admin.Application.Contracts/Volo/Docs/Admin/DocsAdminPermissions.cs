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

        public static string[] GetAll()
        {
            return new[]
            {
                GroupName,
                Projects.Default,
                Projects.Delete,
                Projects.Update,
                Projects.Create,
            };
        }
    }
}
