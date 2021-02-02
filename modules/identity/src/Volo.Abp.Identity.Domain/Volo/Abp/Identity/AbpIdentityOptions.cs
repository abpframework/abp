namespace Volo.Abp.Identity
{
    public class AbpIdentityOptions
    {
        public ExternalLoginProviderDictionary ExternalLoginProviders { get; }

        /// <summary>
        /// Default: true.
        /// </summary>
        public bool IsDistributedEventHandlingEnabled { get; } = true;

        public AbpIdentityOptions()
        {
            ExternalLoginProviders = new ExternalLoginProviderDictionary();
        }
    }
}
