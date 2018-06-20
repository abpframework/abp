namespace Volo.Abp.Http.Client
{
    public class RemoteServiceOptions
    {
        public RemoteServiceConfigurationDictionary RemoteServices { get; set; }

        public RemoteServiceOptions()
        {
            RemoteServices = new RemoteServiceConfigurationDictionary();
        }
    }
}
