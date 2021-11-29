using Volo.Abp.Data;

namespace Volo.Abp.BlobStoring.Database
{
    public static class BlobStoringDatabaseDbProperties
    {
        public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "AbpBlobStoring";
    }
}
