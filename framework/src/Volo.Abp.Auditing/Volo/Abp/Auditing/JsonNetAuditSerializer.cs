using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Auditing
{
    public class JsonNetAuditSerializer : IAuditSerializer, ITransientDependency
    {
        protected AbpAuditingOptions Options;

        public JsonNetAuditSerializer(IOptions<AbpAuditingOptions> options)
        {
            Options = options.Value;
        }

        public string Serialize(object obj)
        {
            var options = new JsonSerializerSettings
            {
                ContractResolver = new AuditingContractResolver(Options.IgnoredTypes)
            };

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}