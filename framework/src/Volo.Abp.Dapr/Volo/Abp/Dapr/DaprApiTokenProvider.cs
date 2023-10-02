using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Dapr;

public class DaprApiTokenProvider : IDaprApiTokenProvider, ISingletonDependency
{
    protected AbpDaprOptions Options { get; }

    public DaprApiTokenProvider(IOptions<AbpDaprOptions> options)
    {
        Options = options.Value;
    }

    public virtual string? GetDaprApiToken()
    {
        return Options.DaprApiToken;
    }

    public virtual string? GetAppApiToken()
    {
        return Options.AppApiToken;
    }
}
