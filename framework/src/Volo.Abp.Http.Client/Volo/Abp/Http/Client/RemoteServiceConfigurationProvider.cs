using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client
{
    public class RemoteServiceConfigurationProvider : IRemoteServiceConfigurationProvider, IScopedDependency
    {
        protected AbpRemoteServiceOptions Options { get; }

        public RemoteServiceConfigurationProvider(IOptionsMonitor<AbpRemoteServiceOptions> options)
        {
            Options = options.CurrentValue;
        }

        public Task<RemoteServiceConfiguration> GetConfigurationOrDefaultAsync(string name)
        {
            return Task.FromResult(Options.RemoteServices.GetConfigurationOrDefault(name));
        }

        public Task<RemoteServiceConfiguration> GetConfigurationOrDefaultOrNullAsync(string name)
        {
            return Task.FromResult(Options.RemoteServices.GetConfigurationOrDefaultOrNull(name));
        }
    }
}
