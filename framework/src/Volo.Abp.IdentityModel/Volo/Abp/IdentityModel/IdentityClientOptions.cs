namespace Volo.Abp.IdentityModel
{
    public class IdentityClientOptions
    {
        public IdentityClientConfigurationDictionary IdentityClients { get; set; }

        public IdentityClientOptions()
        {
            IdentityClients = new IdentityClientConfigurationDictionary();
        }
    }
}