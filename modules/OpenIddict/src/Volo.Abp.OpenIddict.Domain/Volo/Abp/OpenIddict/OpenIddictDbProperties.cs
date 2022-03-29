namespace Volo.Abp.OpenIddict;

public static class OpenIddictDbProperties
{
    public static string DbTablePrefix { get; set; } = "AbpOpenIddict";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "AbpOpenIddict";
}
