namespace Volo.Abp.AzureServiceBus
{
    public class AbpAzureServiceBusOptions
    {
        public AzureServiceBusConnections Connections { get; }

        public AbpAzureServiceBusOptions()
        {
            Connections = new AzureServiceBusConnections();
        }
    }
}