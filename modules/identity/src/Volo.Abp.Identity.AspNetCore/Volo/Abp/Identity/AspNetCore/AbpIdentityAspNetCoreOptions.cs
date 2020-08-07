namespace Volo.Abp.Identity.AspNetCore
{
    public class AbpIdentityAspNetCoreOptions
    {
        /// <summary>
        /// Default: true.
        /// </summary>
        public bool ConfigureAuthentication { get; set; } = true;

        public ExternalLoginProviderDictionary ExternalLoginProviders { get; }

        public AbpIdentityAspNetCoreOptions()
        {
            ExternalLoginProviders = new ExternalLoginProviderDictionary();
        }
    }
}
