using System.Net.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class DefaultDynamicProxyHttpClientFactory : IDynamicProxyHttpClientFactory, ITransientDependency
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }
    }
}