using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.ObjectMapping;
using ApiResource = IdentityServer4.Models.ApiResource;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Volo.Abp.IdentityServer
{
    public class ResourceStore : IResourceStore
    {
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IObjectMapper<AbpIdentityServerDomainModule> _objectMapper;

        public ResourceStore(
            IIdentityResourceRepository identityResourceRepository, 
            IObjectMapper<AbpIdentityServerDomainModule> objectMapper, 
            IApiResourceRepository apiResourceRepository)
        {
            _identityResourceRepository = identityResourceRepository;
            _objectMapper = objectMapper;
            _apiResourceRepository = apiResourceRepository;
        }

        public virtual async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var resource = await _identityResourceRepository.GetListByScopesAsync(scopeNames.ToArray(), includeDetails: true);
            return _objectMapper.Map<List<IdentityResource>, List<IdentityServer4.Models.IdentityResource>>(resource);
        }

        public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var resources = await _apiResourceRepository.GetListByScopesAsync(scopeNames.ToArray(), includeDetails: true);
            return resources.Select(x => _objectMapper.Map<ApiResources.ApiResource, ApiResource>(x));
        }

        public virtual async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var resource = await _apiResourceRepository.FindByNameAsync(name);
            return _objectMapper.Map<ApiResources.ApiResource, ApiResource>(resource);
        }

        public virtual async Task<Resources> GetAllResourcesAsync()
        {
            var identityResources = await _identityResourceRepository.GetListAsync(includeDetails: true);
            var apiResources = await _apiResourceRepository.GetListAsync(includeDetails: true);

            return new Resources(
                _objectMapper.Map<List<IdentityResource>, IdentityServer4.Models.IdentityResource[]>(identityResources),
                _objectMapper.Map<List<ApiResources.ApiResource>, ApiResource[]>(apiResources)
            );
        }
    }
}
