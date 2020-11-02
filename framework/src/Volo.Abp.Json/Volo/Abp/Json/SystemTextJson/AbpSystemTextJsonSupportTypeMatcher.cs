using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.SystemTextJson
{
    public class AbpSystemTextJsonSupportTypeMatcher : ITransientDependency
    {
        protected AbpSystemTextJsonSerializerOptions Options { get; }

        public AbpSystemTextJsonSupportTypeMatcher(IOptions<AbpSystemTextJsonSerializerOptions> options)
        {
            Options = options.Value;
        }

        public virtual bool Match(Type type)
        {
            return !Options.UnsupportedTypes.Contains(type);
        }
    }
}
