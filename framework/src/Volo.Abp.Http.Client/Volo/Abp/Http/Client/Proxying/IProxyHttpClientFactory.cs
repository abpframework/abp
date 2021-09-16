using System.Net.Http;

namespace Volo.Abp.Http.Client.Proxying
{
    public interface IProxyHttpClientFactory
    {
        HttpClient Create();

        HttpClient Create(string name);
    }
}
