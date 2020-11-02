using System;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.SystemTextJson
{
    public class AbpSystemTextJsonSupportTypeMatcher : ITransientDependency
    {
        protected AbpSystemTextJsonSupportTypeMatcherOptions Options { get; }

        public AbpSystemTextJsonSupportTypeMatcher(IOptions<AbpSystemTextJsonSupportTypeMatcherOptions> options)
        {
            Options = options.Value;
        }

        public virtual bool Match(Type type)
        {
            return !Options.UnsupportedTypes.Contains(type);
        }
    }
}
