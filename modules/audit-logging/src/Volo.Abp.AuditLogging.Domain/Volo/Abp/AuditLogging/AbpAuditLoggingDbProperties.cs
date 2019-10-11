using Volo.Abp.Data;

namespace Volo.Abp.AuditLogging
{
    public static class AbpAuditLoggingDbProperties
    {
        public static string DbTablePrefix { get; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpAuditLogging";
    }
}
