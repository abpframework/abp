using Shouldly;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionGrantCacheItem_Tests
{
    [Fact]
    public void GetPermissionNameFormCacheKeyOrNull()
    {
        var key = PermissionGrantCacheItem.CalculateCacheKey("aaa", "bbb", "ccc");
        PermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key).ShouldBe("aaa");
        PermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull("aaabbbccc").ShouldBeNull();
    }
}
