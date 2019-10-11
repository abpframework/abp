using Volo.Abp.Data;

namespace Volo.Abp.FeatureManagement
{
    public static class FeatureManagementDbProperties
    {
        public static string DbTablePrefix { get; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpFeatureManagement";
    }
}
