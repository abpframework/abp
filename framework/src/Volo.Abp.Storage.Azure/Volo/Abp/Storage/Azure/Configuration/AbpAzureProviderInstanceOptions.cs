using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Azure.Configuration
{
    public class AbpAzureProviderInstanceOptions : ProviderInstanceOptions
    {
        public string ConnectionString { get; set; }

        public string ConnectionStringName { get; set; }
    }
}