using System.Net.Http;

namespace Volo.Abp.Http.DynamicProxying
{
    public interface IDynamicProxyHttpClientFactory
    {
        HttpClient Create();
    }
}