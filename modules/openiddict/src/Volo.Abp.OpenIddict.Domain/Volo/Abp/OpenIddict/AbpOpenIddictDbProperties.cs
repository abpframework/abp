using Volo.Abp.Data;

namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictDbProperties
{
    public static string DbTablePrefix { get; set; } = "OpenIddict";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpOpenIddict";
}
