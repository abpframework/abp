using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations
{
    [Area("Abp")]
    [Route("Abp/ApplicationConfigurationScript")]
    public class AbpApplicationConfigurationScriptController : AbpController
    {
        private readonly IApplicationConfigurationBuilder _configurationBuilder;
        private readonly IJsonSerializer _jsonSerializer;

        public AbpApplicationConfigurationScriptController(
            IApplicationConfigurationBuilder configurationBuilder,
            IJsonSerializer jsonSerializer)
        {
            _configurationBuilder = configurationBuilder;
            _jsonSerializer = jsonSerializer;
        }

        [HttpGet]
        [Produces("text/javascript", "text/plain")]
        public async Task<string> Get()
        {
            var config = await _configurationBuilder.Get();
            return CreateAbpExtendScript(config);
        }

        private string CreateAbpExtendScript(ApplicationConfigurationDto config)
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");

            script.AppendLine($"$.extend(true, abp, {_jsonSerializer.Serialize(config, indented: Debugger.IsAttached)})");

            script.Append("})();");

            return script.ToString();
        }
    }
}
