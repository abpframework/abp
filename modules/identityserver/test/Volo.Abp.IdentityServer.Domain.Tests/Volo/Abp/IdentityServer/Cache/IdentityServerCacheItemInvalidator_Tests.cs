using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Reflection;
using Xunit;
using ApiResource = IdentityServer4.Models.ApiResource;
using ApiScope = IdentityServer4.Models.ApiScope;
using Client = IdentityServer4.Models.Client;
using IdentityResource = IdentityServer4.Models.IdentityResource;

namespace Volo.Abp.IdentityServer.Cache
{
    public class IdentityServerCacheItemInvalidator_Tests : AbpIdentityServerTestBase
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;

        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IApiScopeRepository _apiScopeRepository;

        private readonly IDistributedCache<IdentityServerDistributedCacheItem<Client>> _clientCache;
        private readonly IDistributedCache<IdentityServerDistributedCacheItem<IEnumerable<IdentityResource>>> _identityResourceCache;
        private readonly IDistributedCache<IdentityServerDistributedCacheItem<IEnumerable<ApiResource>>> _apiResourceCache;
        private readonly IDistributedCache<IdentityServerDistributedCacheItem<IEnumerable<ApiScope>>> _apiScopeCache;
        private readonly IDistributedCache<IdentityServerDistributedCacheItem<Resources>> _resourceCache;

        private readonly ICorsPolicyService _corsPolicyService;
        private readonly IDistributedCache<IdentityServerDistributedCacheItem<CachingCorsPolicyService<AbpCorsPolicyService>.CorsCacheEntry>> _corsPolicyServiceCache;

        private readonly AbpIdentityServerTestData _testData;

        public IdentityServerCacheItemInvalidator_Tests()
        {
            _clientStore = GetRequiredService<IClientStore>();
            _resourceStore = GetRequiredService<IResourceStore>();

            _clientRepository = GetRequiredService<IClientRepository>();
            _identityResourceRepository = GetRequiredService<IIdentityResourceRepository>();
            _apiResourceRepository = GetRequiredService<IApiResourceRepository>();
            _apiScopeRepository = GetRequiredService<IApiScopeRepository>();

            _clientCache = GetRequiredService<IDistributedCache<IdentityServerDistributedCacheItem<Client>>>();
            _identityResourceCache = GetRequiredService<IDistributedCache<IdentityServerDistributedCacheItem<IEnumerable<IdentityResource>>>>();
            _apiResourceCache = GetRequiredService<IDistributedCache<IdentityServerDistributedCacheItem<IEnumerable<ApiResource>>>>();
            _apiScopeCache = GetRequiredService<IDistributedCache<IdentityServerDistributedCacheItem<IEnumerable<ApiScope>>>>();
            _resourceCache =  GetRequiredService<IDistributedCache<IdentityServerDistributedCacheItem<Resources>>>();

            _corsPolicyService = GetRequiredService<ICorsPolicyService>();
            _corsPolicyServiceCache = GetRequiredService<IDistributedCache<IdentityServerDistributedCacheItem<CachingCorsPolicyService<AbpCorsPolicyService>.CorsCacheEntry>>>();

            _testData = GetRequiredService<AbpIdentityServerTestData>();
        }

        [Fact]
        public async Task Models_Should_Cached_And_Invalidator_When_Its_Changed()
        {
            //client
            var clientId = "ClientId1";
            var clientIdCacheKey = GetTestCacheKey<Client>(clientId);
            (await _clientCache.GetAsync(clientIdCacheKey)).ShouldBeNull();

            var client = await _clientStore.FindClientByIdAsync(clientId);
            client.ShouldNotBeNull();

            var clientCacheItem = await _clientCache.GetAsync(clientIdCacheKey);
            clientCacheItem.ShouldNotBeNull();

            await _clientRepository.DeleteAsync(_testData.Client1Id);
            (await _clientCache.GetAsync(clientIdCacheKey)).ShouldBeNull();


            //Api Resource
            var newApiResource1 = "NewApiResource1";
            var newApiResource2 = "NewApiResource2";
            var testApiResourceName1 = "Test-ApiResource-Name-1";
            var testApiResourceApiScopeName1 = "Test-ApiResource-ApiScope-Name-1";

            var newApiResources = new[] {newApiResource1, newApiResource2};
            var newApiResourcesCacheKey = GetTestCacheKey<IEnumerable<ApiResource>>(newApiResources);
            var testApiResourceApiScopeName1CacheKey = GetTestCacheKey<IEnumerable<ApiResource>>(new []{ testApiResourceApiScopeName1 });

            //FindApiResourcesByNameAsync
            (await _apiResourceCache.GetAsync(newApiResourcesCacheKey)).ShouldBeNull();
            await _resourceStore.FindApiResourcesByNameAsync(newApiResources);
            (await _apiResourceCache.GetAsync(newApiResourcesCacheKey)).ShouldNotBeNull();

            var apiResource1 = await _apiResourceRepository.FindByNameAsync(newApiResource1);
            await _apiResourceRepository.DeleteAsync(apiResource1);
            (await _apiResourceCache.GetAsync(newApiResourcesCacheKey)).ShouldBeNull();

            //FindApiResourcesByScopeNameAsync
            (await _apiResourceCache.GetAsync(testApiResourceApiScopeName1CacheKey)).ShouldBeNull();
            await _resourceStore.FindApiResourcesByScopeNameAsync(new []{ testApiResourceApiScopeName1 });
            (await _apiResourceCache.GetAsync(testApiResourceApiScopeName1CacheKey)).ShouldNotBeNull();

            var testApiResource1 = await _apiResourceRepository.FindByNameAsync(testApiResourceName1);
            await _apiResourceRepository.DeleteAsync(testApiResource1);
            (await _apiResourceCache.GetAsync(testApiResourceApiScopeName1CacheKey)).ShouldBeNull();


            //Identity Resource
            var testIdentityResourceName = "Test-Identity-Resource-Name-1";
            var testIdentityResourceNames = new[] {testIdentityResourceName};
            var testIdentityResourceNamesCacheKey = GetTestCacheKey<IEnumerable<IdentityResource>>(testIdentityResourceNames);
            (await _identityResourceCache.GetAsync(testIdentityResourceNamesCacheKey)).ShouldBeNull();
            await _resourceStore.FindIdentityResourcesByScopeNameAsync(testIdentityResourceNames);
            (await _identityResourceCache.GetAsync(testIdentityResourceNamesCacheKey)).ShouldNotBeNull();

            var testIdentityResource = await _identityResourceRepository.FindByNameAsync(testIdentityResourceName);
            await _identityResourceRepository.DeleteAsync(testIdentityResource);
            (await _identityResourceCache.GetAsync(testIdentityResourceNamesCacheKey)).ShouldBeNull();


            //Api Scope
            var testApiScopeName = "Test-ApiScope-Name-1";
            var testApiScopeNames = new[] {testApiScopeName};
            var testApiScopeNamesCacheKey = GetTestCacheKey<IEnumerable<ApiScope>>(testApiScopeNames);
            (await _apiScopeCache.GetAsync(testApiScopeNamesCacheKey)).ShouldBeNull();
            await _resourceStore.FindApiScopesByNameAsync(testApiScopeNames);
            (await _apiScopeCache.GetAsync(testApiScopeNamesCacheKey)).ShouldNotBeNull();

            var testApiScope = await _apiScopeRepository.GetByNameAsync(testApiScopeName);
            await _apiScopeRepository.DeleteAsync(testApiScope);
            (await _apiScopeCache.GetAsync(testApiScopeNamesCacheKey)).ShouldBeNull();

            //Resources
            var allKey = GetTestCacheKey<Resources>("__all__");
            (await _resourceCache.GetAsync(allKey)).ShouldBeNull();
            await _resourceStore.GetAllResourcesAsync();
            (await _resourceCache.GetAsync(allKey)).ShouldNotBeNull();

            await _identityResourceRepository.DeleteAsync(_testData.IdentityResource1Id);
            (await _resourceCache.GetAsync(allKey)).ShouldBeNull();

            //CorsPolicy Service
            var origin = "https://abp.io";
            var originCacheKey = GetTestCacheKey<CachingCorsPolicyService<AbpCorsPolicyService>.CorsCacheEntry>(origin);
            (await _corsPolicyServiceCache.GetAsync(originCacheKey)).ShouldBeNull();
            await _corsPolicyService.IsOriginAllowedAsync(origin);
            (await _corsPolicyServiceCache.GetAsync(originCacheKey)).ShouldNotBeNull();
        }

        private string GetTestCacheKey<T>(IEnumerable<string> names)
            where T : class
        {
            if (names == null || !names.Any())
            {
                return string.Empty;
            }

            return TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(T)) + ":" + names.OrderBy(x => x).Aggregate((x, y) => x + "," + y);
        }

        private string GetTestCacheKey<T>(string key)
            where T : class
        {
            return TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(T)) + ":" + key;
        }
    }
}
