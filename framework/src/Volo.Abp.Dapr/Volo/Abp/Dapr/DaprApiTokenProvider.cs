using System.Threading.Tasks;
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

    public virtual Task<string> GetDaprApiTokenAsync()
    {
        return Task.FromResult(Options.DaprApiToken);
    }

    public virtual Task<string> GetAppApiTokenAsync()
    {
        return Task.FromResult(Options.AppApiToken);
    }
}
