using System.Text.Json;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    //TODO: Rename to JsonAuditSerializer
    public class JsonNetAuditSerializer : IAuditSerializer, ITransientDependency
    {
        protected AbpAuditingOptions Options;

        public JsonNetAuditSerializer(IOptions<AbpAuditingOptions> options)
        {
            Options = options.Value;
        }

        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, GetJsonSerializerOptions());
        }

        private JsonSerializerOptions GetJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new AuditingRuntimeIgnoreConverter(Options.IgnoredTypes));
            return options;
        }
    }
}
