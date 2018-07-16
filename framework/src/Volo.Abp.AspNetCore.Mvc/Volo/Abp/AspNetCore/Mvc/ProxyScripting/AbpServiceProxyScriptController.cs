using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Http.ProxyScripting;

namespace Volo.Abp.AspNetCore.Mvc.ProxyScripting
{
    [Area("Abp")]
    [Route("Abp/ServiceProxyScript")]
    [DisableAuditing]
    public class AbpServiceProxyScriptController : AbpController
    {
        private readonly IProxyScriptManager _proxyScriptManager;

        public AbpServiceProxyScriptController(IProxyScriptManager proxyScriptManager)
        {
            _proxyScriptManager = proxyScriptManager;
        }

        [HttpGet]
        [Produces("text/javascript", "text/plain")]
        public string GetAll(ServiceProxyGenerationModel model)
        {
            model.Normalize();
            return _proxyScriptManager.GetScript(model.CreateOptions());
        }
    }
}
