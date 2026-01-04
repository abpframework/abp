using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Caching;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionGrantCacheItemInvalidator_Tests : PermissionTestBase
{
    private readonly IDistributedCache<ResourcePermissionGrantCacheItem> _cache;
    private readonly IResourcePermissionStore _resourcePermissionStore;
    private readonly IResourcePermissionGrantRepository _resourcePermissionGrantRepository;

    public ResourcePermissionGrantCacheItemInvalidator_Tests()
    {
        _cache = GetRequiredService<IDistributedCache<ResourcePermissionGrantCacheItem>>();
        _resourcePermissionStore = GetRequiredService<IResourcePermissionStore>();
        _resourcePermissionGrantRepository = GetRequiredService<IResourcePermissionGrantRepository>();
    }

    [Fact]
    public async Task PermissionStore_IsGrantedAsync_Should_Cache_PermissionGrant()
    {
        (await _cache.GetAsync(ResourcePermissionGrantCacheItem.CalculateCacheKey("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString()))).ShouldBeNull();

        await _resourcePermissionStore.IsGrantedAsync("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        (await _cache.GetAsync(ResourcePermissionGrantCacheItem.CalculateCacheKey("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString()))).ShouldNotBeNull();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_WhenPermissionGrantChanged()
    {
        // IsGrantedAsync will cache ResourcePermissionGrant
        await _resourcePermissionStore.IsGrantedAsync("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        var resourcePermissionGrant = await _resourcePermissionGrantRepository.FindAsync("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());
        resourcePermissionGrant.ShouldNotBeNull();
        await _resourcePermissionGrantRepository.DeleteAsync(resourcePermissionGrant);

        (await _cache.GetAsync(ResourcePermissionGrantCacheItem.CalculateCacheKey("MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString()))).ShouldBeNull();
    }
}
