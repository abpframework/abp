using System.Net.Http;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public interface IDynamicProxyHttpClientFactory
    {
        HttpClient Create();
    }
}