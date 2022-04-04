namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictDbProperties
{
    public static string DbTablePrefix { get; set; } = "AbpOpenIddict"; //TODO: Rename to "OpenIddict" 

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "AbpOpenIddict"; //TODO: Rename to "OpenIddict"
}
