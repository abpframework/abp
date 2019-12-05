using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
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
        [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
        public ActionResult GetAll(ServiceProxyGenerationModel model)
        {
            model.Normalize();
            return Content(_proxyScriptManager.GetScript(model.CreateOptions()), MimeTypes.Application.Javascript);
        }
    }
}
