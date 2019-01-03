using System.Net.Http;

namespace Volo.Abp.Http.Client.Authentication
{
    public class HttpClientAuthenticateContext
    {
        public HttpClient Client { get; }

        public HttpRequestMessage Request { get; }

        public RemoteServiceConfiguration RemoteService { get; }

        public HttpClientAuthenticateContext(
            HttpClient client, 
            HttpRequestMessage request,
            RemoteServiceConfiguration remoteService)
        {
            Client = client;
            Request = request;
            RemoteService = remoteService;
        }
    }
}