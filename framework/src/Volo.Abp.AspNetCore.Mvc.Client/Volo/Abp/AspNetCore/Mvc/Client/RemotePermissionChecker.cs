using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    public class RemotePermissionChecker : IPermissionChecker, ITransientDependency
    {
        protected ICachedApplicationConfigurationClient ConfigurationClient { get; }

        public RemotePermissionChecker(ICachedApplicationConfigurationClient configurationClient)
        {
            ConfigurationClient = configurationClient;
        }

        public async Task<PermissionGrantInfo> CheckAsync(string name)
        {
            var configuration = await ConfigurationClient.GetAsync();

            return new PermissionGrantInfo(
                name,
                configuration.Auth.GrantedPolicies.ContainsKey(name)
            );
        }

        public Task<PermissionGrantInfo> CheckAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            return CheckAsync(name);
        }
    }
}
