using System.Threading.Tasks;
using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiAndIdentityResources
    {
        public Task<IdentityResource[]> IdentityResources { get; set; }

        public Task<ApiResource[]> ApiResources { get; set; }

        public ApiAndIdentityResources(Task<IdentityResource[]> identityResources, Task<ApiResource[]> apiResources)
        {
            IdentityResources = identityResources;
            ApiResources = apiResources;
        }
    }
}