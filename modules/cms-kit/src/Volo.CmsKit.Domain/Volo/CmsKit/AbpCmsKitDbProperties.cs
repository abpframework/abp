namespace Volo.CmsKit;

public static class AbpCmsKitDbProperties
{
    public static string DbTablePrefix { get; set; } = "Cms";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "CmsKit";
}
