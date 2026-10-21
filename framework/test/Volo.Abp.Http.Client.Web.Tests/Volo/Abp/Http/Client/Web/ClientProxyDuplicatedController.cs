using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Http.Client.Web;

[Route("api/client-proxy-duplicated")]
public class ClientProxyDuplicatedController : AbpControllerBase, IClientProxyDuplicatedAppService
{
    [HttpGet]
    public virtual Task<string> GetAsync()
    {
        return Task.FromResult("controller");
    }
}
