namespace Volo.Abp.OpenIddict;

public static class AbpOpenIddictDbProperties
{
    public static string DbTablePrefix { get; set; } = "OpenIddict";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "OpenIddict";
}
