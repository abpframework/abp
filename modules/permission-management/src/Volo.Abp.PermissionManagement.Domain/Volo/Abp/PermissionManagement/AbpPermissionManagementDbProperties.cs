using Volo.Abp.Data;

namespace Volo.Abp.PermissionManagement
{
    public static class AbpPermissionManagementDbProperties
    {
        public static string DbTablePrefix { get; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpPermissionManagement";
    }
}
