using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceStore : IResourceStore, ITransientDependency
    {
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IObjectMapper _objectMapper;

        public IdentityResourceStore(IIdentityResourceRepository identityResourceRepository, IObjectMapper objectMapper)
        {
            _identityResourceRepository = identityResourceRepository;
            _objectMapper = objectMapper;
        }

        public virtual async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var results = await _identityResourceRepository.FindIdentityResourcesByScopeAsync(scopeNames.ToArray());
            return _objectMapper.Map<List<IdentityResource>, List<IdentityServer4.Models.IdentityResource>>(results);
        }

        public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var results = await _identityResourceRepository.FindApiResourcesByScopeAsync(scopeNames.ToArray());
            return results?.Select(x => _objectMapper.Map<ApiResources.ApiResource, ApiResource>(x));
        }

        public virtual async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var result = await _identityResourceRepository.FindApiResourceAsync(name);
            return _objectMapper.Map<ApiResources.ApiResource, ApiResource>(result);
        }

        public virtual async Task<Resources> GetAllResourcesAsync()
        {
            var result = await _identityResourceRepository.GetAllResourcesAsync();

            return new Resources(
                _objectMapper.Map<IdentityResource[], IdentityServer4.Models.IdentityResource[]>(result.IdentityResources),
                _objectMapper.Map<ApiResources.ApiResource[], ApiResource[]>(result.ApiResources)
            );
        }
    }
}
