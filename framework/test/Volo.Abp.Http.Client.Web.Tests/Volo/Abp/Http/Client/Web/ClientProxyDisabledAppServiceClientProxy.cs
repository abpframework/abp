using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

[RemoteService(false)]
public class ClientProxyDisabledAppServiceClientProxy : ClientProxyBase<IClientProxyDisabledAppService>, IClientProxyDisabledAppService
{
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("disabled");
    }

    [HttpGet("api/client-proxy-disabled/with-route")]
    public virtual Task<string> GetWithRouteAsync()
    {
        return Task.FromResult("disabled-with-route");
    }
}
