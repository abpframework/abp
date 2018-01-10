using Volo.Abp.IdentityServer.IdentityResources;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiAndIdentityResources
    {
        public IdentityResource[] IdentityResources { get; set; }

        public ApiResource[] ApiResources { get; set; }

        public ApiAndIdentityResources(IdentityResource[] identityResources, ApiResource[] apiResources)
        {
            IdentityResources = identityResources;
            ApiResources = apiResources;
        }
    }
}