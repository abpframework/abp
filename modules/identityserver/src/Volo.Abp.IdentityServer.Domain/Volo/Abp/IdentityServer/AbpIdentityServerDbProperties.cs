namespace Volo.Abp.IdentityServer
{
    public static class AbpIdentityServerDbProperties
    {
        public static string DbTablePrefix { get; } = "IdentityServer";

        public static string DbSchema { get; } = null;

        public const string ConnectionStringName = "AbpIdentityServer";
    }
}
