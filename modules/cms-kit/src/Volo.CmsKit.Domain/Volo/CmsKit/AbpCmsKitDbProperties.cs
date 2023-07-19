using Volo.Abp.Data;

namespace Volo.CmsKit;

public static class AbpCmsKitDbProperties
{
    public static string DbTablePrefix { get; set; } = "Cms";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "CmsKit";
}
