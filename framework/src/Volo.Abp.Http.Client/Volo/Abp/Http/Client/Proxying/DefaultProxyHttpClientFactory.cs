using System.Net.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.Proxying
{
    public class DefaultProxyHttpClientFactory : IProxyHttpClientFactory, ITransientDependency
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultProxyHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Create()
        {
            return _httpClientFactory.CreateClient();
        }

        public HttpClient Create(string name)
        {
            return _httpClientFactory.CreateClient(name);
        }
    }
}
