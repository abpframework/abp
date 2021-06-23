using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client
{
    public class RemoteServiceConfigurationProvider : IRemoteServiceConfigurationProvider, IScopedDependency
    {
        protected AbpRemoteServiceOptions Options { get; }

        public RemoteServiceConfigurationProvider(IOptionsSnapshot<AbpRemoteServiceOptions> options)
        {
            Options = options.Value;
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