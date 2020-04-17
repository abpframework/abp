namespace Volo.Abp.IdentityServer
{
    public class AllowedCorsOriginsCacheItem
    {
        public const string AllOrigins = "AllOrigins";

        public string[] AllowedOrigins { get; set; }
    }
}