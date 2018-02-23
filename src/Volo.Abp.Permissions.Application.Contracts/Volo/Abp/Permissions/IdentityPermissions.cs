namespace Volo.Abp.Permissions
{
    public static class PermissionPermissions
    {
        public const string GroupName = "AbpPermissions";

        public static class Permissions
        {
            public const string Default = GroupName + ".Permissions";
            public const string Update = Default + ".Update";
        }
    }
}