using System.Net.Http;
using System.Threading.Tasks;

namespace Volo.Abp.Http.Client.Proxying
{
    public interface IHttpProxyExecuter
    {
        Task<HttpContent> MakeRequestAsync(HttpProxyExecuterContext context);

        Task<T> MakeRequestAndGetResultAsync<T>(HttpProxyExecuterContext context);
    }
}
