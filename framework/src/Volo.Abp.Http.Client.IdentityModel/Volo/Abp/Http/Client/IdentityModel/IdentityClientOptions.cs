namespace Volo.Abp.Http.Client.IdentityModel
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