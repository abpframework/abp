namespace Volo.Abp.Storage.Configuration
{
    public class AzureProviderInstanceOptions : ProviderInstanceOptions
    {
        public string ConnectionString { get; set; }

        public string ConnectionStringName { get; set; }
    }
}
