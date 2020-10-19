using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.IdentityServer
{
    public class ResourceStore : IResourceStore
    {
        protected IIdentityResourceRepository IdentityResourceRepository { get; }
        protected IApiResourceRepository ApiResourceRepository { get; }
        protected IApiScopeRepository ApiScopeRepository { get; }
        protected IObjectMapper<AbpIdentityServerDomainModule> ObjectMapper { get; }

        public ResourceStore(
            IIdentityResourceRepository identityResourceRepository,
            IObjectMapper<AbpIdentityServerDomainModule> objectMapper,
            IApiResourceRepository apiResourceRepository,
            IApiScopeRepository apiScopeRepository)
        {
            IdentityResourceRepository = identityResourceRepository;
            ObjectMapper = objectMapper;
            ApiResourceRepository = apiResourceRepository;
            ApiScopeRepository = apiScopeRepository;
        }

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        public virtual async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resource = await IdentityResourceRepository.GetListByScopeNameAsync(scopeNames.ToArray(), includeDetails: true);
            return ObjectMapper.Map<List<Volo.Abp.IdentityServer.IdentityResources.IdentityResource>, List<IdentityServer4.Models.IdentityResource>>(resource);
        }

        /// <summary>
        /// Gets API scopes by scope name.
        /// </summary>
        public virtual async Task<IEnumerable<IdentityServer4.Models.ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = await ApiScopeRepository.GetListByNameAsync(scopeNames.ToArray(), includeDetails: true);
            return ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiScopes.ApiScope>, List<IdentityServer4.Models.ApiScope>>(scopes);
        }

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        public virtual async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = await ApiResourceRepository.GetListByScopesAsync(scopeNames.ToArray(), includeDetails: true);
            return ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiResources.ApiResource>, List<IdentityServer4.Models.ApiResource>>(resources);
        }

        /// <summary>
        /// Gets API resources by API resource name.
        /// </summary>
        public virtual async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var resources = await ApiResourceRepository.FindByNameAsync(apiResourceNames.ToArray(), includeDetails: true);
            return ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiResources.ApiResource>, List<IdentityServer4.Models.ApiResource>>(resources);
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        public virtual async Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            var identityResources = await IdentityResourceRepository.GetListAsync(includeDetails: true);
            var apiResources = await ApiResourceRepository.GetListAsync(includeDetails: true);
            var apiScopes = await ApiScopeRepository.GetListAsync(includeDetails: true);

            return new Resources(
                ObjectMapper.Map<List<Volo.Abp.IdentityServer.IdentityResources.IdentityResource>, List<IdentityServer4.Models.IdentityResource>>(identityResources),
                ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiResources.ApiResource>, List<IdentityServer4.Models.ApiResource>>(apiResources),
                ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiScopes.ApiScope>, List<IdentityServer4.Models.ApiScope>>(apiScopes));
        }
    }
}
