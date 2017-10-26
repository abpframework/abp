using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Http.ProxyScripting;

namespace Volo.Abp.AspNetCore.Mvc.ProxyScripting
{
    //TODO: abp area?
    //TODO: [DisableAuditing]
    public class AbpServiceProxiesController : AbpController
    {
        private readonly IProxyScriptManager _proxyScriptManager;

        public AbpServiceProxiesController(IProxyScriptManager proxyScriptManager)
        {
            _proxyScriptManager = proxyScriptManager;
        }

        [Produces("text/javascript", "text/plain")]
        public string GetAll(ServiceProxyGenerationModel model)
        {
            model.Normalize();
            return _proxyScriptManager.GetScript(model.CreateOptions());
        }
    }
}
