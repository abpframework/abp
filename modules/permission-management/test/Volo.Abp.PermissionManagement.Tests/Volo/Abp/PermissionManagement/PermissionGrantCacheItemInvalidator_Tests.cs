using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionGrantCacheItemInvalidator_Tests : PermissionTestBase
    {
        private readonly IDistributedCache<PermissionGrantCacheItem> _cache;
        private readonly IPermissionStore _permissionStore;
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        public PermissionGrantCacheItemInvalidator_Tests()
        {
            _cache = GetRequiredService<IDistributedCache<PermissionGrantCacheItem>>();
            _permissionStore = GetRequiredService<IPermissionStore>();
            _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        }

        [Fact]
        public async Task PermissionStore_IsGrantedAsync_Should_Cache_PermissionGrant()
        {
            (await _cache.GetAsync(PermissionGrantCacheItem.CalculateCacheKey("MyPermission1",
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString()))).ShouldBeNull();


            await _permissionStore.IsGrantedAsync("MyPermission1",
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());


            (await _cache.GetAsync(PermissionGrantCacheItem.CalculateCacheKey("MyPermission1",
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString()))).ShouldNotBeNull();
        }

        [Fact]
        public async Task Cache_Should_Invalidator_WhenPermissionGrantChanged()
        {
            // IsGrantedAsync will cache PermissionGrant
            await _permissionStore.IsGrantedAsync("MyPermission1",
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());

            var permissionGrant = await _permissionGrantRepository.FindAsync("MyPermission1",
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());
            permissionGrant.ShouldNotBeNull();
            await _permissionGrantRepository.DeleteAsync(permissionGrant);

            (await _cache.GetAsync(PermissionGrantCacheItem.CalculateCacheKey("MyPermission1",
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString()))).ShouldBeNull();
        }
    }
}
