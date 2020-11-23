using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
            return JsonConvert.SerializeObject(obj, GetSharedJsonSerializerSettings());
        }

        private static readonly object SyncObj = new object();
        private static JsonSerializerSettings _sharedJsonSerializerSettings;

        private JsonSerializerSettings GetSharedJsonSerializerSettings()
        {
            if (_sharedJsonSerializerSettings == null)
            {
                lock (SyncObj)
                {
                    if (_sharedJsonSerializerSettings == null)
                    {
                        _sharedJsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new AuditingContractResolver(Options.IgnoredTypes)
                        };
                    }
                }
            }

            return _sharedJsonSerializerSettings;
        }
    }
}
