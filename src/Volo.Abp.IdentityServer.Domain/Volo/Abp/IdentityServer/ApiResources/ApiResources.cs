using System.Threading.Tasks;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResources
    {
        public Task<IdentityResource[]> Resources { get; set; }

        public Task<ApiResource[]> IdentityResources { get; set; }

        public ApiResources()
        {

        }

        public ApiResources(Task<IdentityResource[]> resources, Task<ApiResource[]> identityResources)
        {
            Resources = resources;
            IdentityResources = identityResources;
        }

    }
}