using Volo.Abp.Data;

namespace Volo.Abp.TenantManagement
{
    public static class AbpTenantManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpTenantManagement";

    }
}
