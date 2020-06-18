namespace Volo.Abp.Http.Client
{
    public class AbpRemoteServiceOptions
    {
        public RemoteServiceConfigurationDictionary RemoteServices { get; set; }

        public AbpRemoteServiceOptions()
        {
            RemoteServices = new RemoteServiceConfigurationDictionary();
        }
    }
}
