namespace Volo.Abp.Http.Client
{
    public class RemoteServiceConfiguration
    {
        public string BaseUrl { get; set; }

        public string Version { get; set; }

        public RemoteServiceConfiguration()
        {
            
        }

        public RemoteServiceConfiguration(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}