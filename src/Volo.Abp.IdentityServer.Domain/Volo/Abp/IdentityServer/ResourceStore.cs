using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.ObjectMapping;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Volo.Abp.IdentityServer
{
    public class ResourceStore : IResourceStore, ITransientDependency
    {
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IObjectMapper _objectMapper;

        public ResourceStore(IIdentityResourceRepository identityResourceRepository, IObjectMapper objectMapper)
        {
            _identityResourceRepository = identityResourceRepository;
            _objectMapper = objectMapper;
        }

        public virtual async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var resource = await _identityResourceRepository.FindIdentityResourcesByScopeAsync(scopeNames.ToArray());
            return _objectMapper.Map<List<IdentityResource>, List<IdentityServer4.Models.IdentityResource>>(resource);
        }

        public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var resources = await _identityResourceRepository.FindApiResourcesByScopeAsync(scopeNames.ToArray());
            return resources?.Select(x => _objectMapper.Map<ApiResources.ApiResource, ApiResource>(x));
        }

        public virtual async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var resource = await _identityResourceRepository.FindApiResourceAsync(name);
            return _objectMapper.Map<ApiResources.ApiResource, ApiResource>(resource);
        }

        public virtual async Task<Resources> GetAllResourcesAsync()
        {
            var resources = await _identityResourceRepository.GetAllResourcesAsync();

            return new Resources(
                _objectMapper.Map<IdentityResource[], IdentityServer4.Models.IdentityResource[]>(resources.IdentityResources),
                _objectMapper.Map<ApiResources.ApiResource[], ApiResource[]>(resources.ApiResources)
            );
        }
    }
}
