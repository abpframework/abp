using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Auditing;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Area("Abp")]
    [Route("Abp/ApplicationConfigurationScript")]
    [DisableAuditing]
    public class AbpApplicationConfigurationScriptController : AbpController
    {
        private readonly IAbpApplicationConfigurationAppService _configurationAppService;
        private readonly IJsonSerializer _jsonSerializer;

        public AbpApplicationConfigurationScriptController(
            IAbpApplicationConfigurationAppService configurationAppService,
            IJsonSerializer jsonSerializer)
        {
            _configurationAppService = configurationAppService;
            _jsonSerializer = jsonSerializer;
        }

        [HttpGet]
        [Produces(MimeTypes.Application.Javascript, MimeTypes.Text.Plain)]
        public async Task<ActionResult> Get()
        {
            Logger.LogDebug("Executing AbpApplicationConfigurationScriptController.Get()");

            var result = CreateAbpExtendScript(
                await _configurationAppService.GetAsync()
            );

            Logger.LogDebug("Executed AbpApplicationConfigurationScriptController.Get()");
            
            return Content(result, MimeTypes.Application.Javascript);
        }

        private string CreateAbpExtendScript(ApplicationConfigurationDto config)
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();
            script.AppendLine($"$.extend(true, abp, {_jsonSerializer.Serialize(config, indented: Debugger.IsAttached)})");
            script.AppendLine();
            script.AppendLine("abp.event.trigger('abp.configurationInitialized');");
            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
