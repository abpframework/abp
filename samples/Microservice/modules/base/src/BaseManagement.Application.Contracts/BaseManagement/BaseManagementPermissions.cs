namespace BaseManagement
{
    public class BaseManagementPermissions
    {
        public const string GroupName = "BaseManagement";

        public static class BaseTypes
        {
            public const string Default = GroupName + ".BaseType";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";

        }

        public static class BaseItems
        {
            public const string Default = GroupName + ".BaseItem";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";

        }
        public static string[] GetAll()
        {
            return new[]
            {
                GroupName,
                BaseTypes.Default,
                BaseTypes.Delete,
                BaseTypes.Update,
                BaseTypes.Create,
                BaseItems.Default,
                BaseItems.Delete,
                BaseItems.Update,
                BaseItems.Create,
            };
        }
    }
}