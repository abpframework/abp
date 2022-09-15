using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Dapr;

public class DaprApiTokenProvider : IDaprApiTokenProvider, ISingletonDependency
{
    public AbpDaprOptions Options { get; }

    public DaprApiTokenProvider(IOptions<AbpDaprOptions> options)
    {
        Options = options.Value;
    }
    
    public virtual string Get()
    {
        return Options.DaprApiToken;
    }
}