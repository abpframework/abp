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
        
        public virtual RemoteServiceConfiguration GetConfigurationOrDefault(string name)
        {
            return Options.RemoteServices.GetConfigurationOrDefault(name);
        }
        
        public virtual RemoteServiceConfiguration GetConfigurationOrDefaultOrNull(string name)
        {
            return Options.RemoteServices.GetConfigurationOrDefaultOrNull(name);
        }
    }
}