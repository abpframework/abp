using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web;

[Route("api/client-proxy-controller-route")]
public class ClientProxyControllerRouteAppServiceClientProxy : ClientProxyBase<IClientProxyControllerRouteAppService>, IClientProxyControllerRouteAppService
{
    [RemoteService(false)]
    [HttpGet]
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("controller-route");
    }
}
