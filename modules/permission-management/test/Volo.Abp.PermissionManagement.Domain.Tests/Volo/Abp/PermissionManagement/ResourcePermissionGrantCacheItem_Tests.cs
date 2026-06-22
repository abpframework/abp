using Shouldly;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionGrantCacheItem_Tests
{
    [Fact]
    public void GetPermissionNameFormCacheKeyOrNull()
    {
        var key = ResourcePermissionGrantCacheItem.CalculateCacheKey("aaa", TestEntityResource.ResourceName, TestEntityResource.ResourceKey1,"bbb", "ccc");
        ResourcePermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key).ShouldBe("aaa");
        ResourcePermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull("aaabbbccc").ShouldBeNull();
    }
}
