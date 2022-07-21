using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.SystemTextJson;

public class AbpSystemTextJsonUnsupportedTypeMatcher : ITransientDependency
{
    protected AbpSystemTextJsonSerializerOptions Options { get; }

    public AbpSystemTextJsonUnsupportedTypeMatcher(IOptions<AbpSystemTextJsonSerializerOptions> options)
    {
        Options = options.Value;
    }

    public virtual bool Match([CanBeNull] Type type)
    {
        return Options.UnsupportedTypes.Contains(type);
    }
}
