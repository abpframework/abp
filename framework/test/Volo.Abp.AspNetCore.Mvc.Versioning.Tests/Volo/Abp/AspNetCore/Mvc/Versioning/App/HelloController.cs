using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{apiVersion:apiVersion}/[controller]")]
    public class HelloController : AbpController, IHelloController
    {
        [HttpPost]
        public Task<string> PostAsync()
        {
            return Task.FromResult($"42-{HttpContext.GetRequestedApiVersion().ToString()}");
        }
    }
}