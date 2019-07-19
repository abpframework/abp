using System;
using System.Collections.Generic;
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
            var options = new JsonSerializerSettings
            {
                ContractResolver = GetSharedAuditingContractResolver(Options.IgnoredTypes)
            };

            return JsonConvert.SerializeObject(obj, options);
        }

        private static readonly object SyncObj = new object();
        private static AuditingContractResolver _sharedAuditingContractResolver;

        public static AuditingContractResolver GetSharedAuditingContractResolver(List<Type> ignoredTypes)
        {
            if (_sharedAuditingContractResolver == null)
            {
                lock (SyncObj)
                {
                    if (_sharedAuditingContractResolver == null)
                    {
                        _sharedAuditingContractResolver = new AuditingContractResolver(ignoredTypes);
                    }
                }
            }

            return _sharedAuditingContractResolver;
        }
    }
}