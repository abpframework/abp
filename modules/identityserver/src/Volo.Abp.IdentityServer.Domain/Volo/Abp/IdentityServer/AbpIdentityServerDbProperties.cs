namespace Volo.Abp.IdentityServer
{
    public static class AbpIdentityServerDbProperties
    {
        public static string DbTablePrefix { get; set; } = "IdentityServer";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "AbpIdentityServer";

    }
}
