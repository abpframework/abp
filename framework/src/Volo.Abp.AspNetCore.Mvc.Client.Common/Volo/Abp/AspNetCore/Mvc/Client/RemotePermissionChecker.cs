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

        public async Task<bool> IsGrantedAsync(string name)
        {
            var configuration = await ConfigurationClient.GetAsync();

            return configuration.Auth.GrantedPolicies.ContainsKey(name);
        }

        public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            /* This provider always works for the current principal. */
            return await IsGrantedAsync(name);
        }

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names)
        {
            var result = new MultiplePermissionGrantResult();
            var configuration = await ConfigurationClient.GetAsync();
            foreach (var name in names)
            {
                result.Result.Add(name, configuration.Auth.GrantedPolicies.ContainsKey(name) ?
                    PermissionGrantResult.Granted :
                    PermissionGrantResult.Undefined);
            }

            return result;
        }

        public async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names)
        {
            /* This provider always works for the current principal. */
            return await IsGrantedAsync(names);
        }
    }
}
