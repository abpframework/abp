using Volo.Abp.Data;

namespace Volo.Docs
{
    public static class AbpDocsDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Docs";

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "Docs";
    }
}
