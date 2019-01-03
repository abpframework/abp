using System.Net.Http;

namespace Volo.Abp.Http.Client.Authentication
{
    public class HttpClientAuthenticateContext
    {
        public HttpClient Client { get; }

        public HttpRequestMessage Request { get; }

        public string RemoteServiceName { get; }

        public HttpClientAuthenticateContext(
            HttpClient client, 
            HttpRequestMessage request, 
            string remoteServiceName)
        {
            Client = client;
            Request = request;
            RemoteServiceName = remoteServiceName;
        }
    }
}