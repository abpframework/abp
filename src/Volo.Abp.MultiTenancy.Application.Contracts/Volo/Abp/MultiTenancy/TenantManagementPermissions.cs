namespace Volo.Abp.MultiTenancy
{
    public static class TenantManagementPermissions
    {
        public const string GroupName = "AbpTenantManagement";

        public static class Tenants
        {
            public const string Default = GroupName + ".Tenants";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}