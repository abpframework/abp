namespace Volo.Abp.IdentityModel
{
    public class AbpIdentityClientOptions
    {
        public IdentityClientConfigurationDictionary IdentityClients { get; set; }

        public AbpIdentityClientOptions()
        {
            IdentityClients = new IdentityClientConfigurationDictionary();
        }
    }
}