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

    [Theory]
    [InlineData("MyModule.Users.Create", "R", "admin")]
    [InlineData("AbpIdentity.Users", "U", "550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("Permission.With.Many.Dots", "T", "tenant-key-123")]
    [InlineData("Simple", "R", "")]
    public void GetPermissionNameFormCacheKeyOrNull_Should_Extract_PermissionName(
        string permissionName, string providerName, string providerKey)
    {
        var key = ResourcePermissionGrantCacheItem.CalculateCacheKey(
            permissionName, TestEntityResource.ResourceName, TestEntityResource.ResourceKey1, providerName, providerKey);
        ResourcePermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key).ShouldBe(permissionName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("no-separator-here")]
    [InlineData("rn:res,rk:key,pn:R,pk:admin")]
    public void GetPermissionNameFormCacheKeyOrNull_Should_Return_Null_For_Invalid_Keys(string invalidKey)
    {
        ResourcePermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(invalidKey).ShouldBeNull();
    }
}
