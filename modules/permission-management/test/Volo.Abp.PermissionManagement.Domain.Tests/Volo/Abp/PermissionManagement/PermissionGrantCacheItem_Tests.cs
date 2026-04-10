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

    [Theory]
    [InlineData("MyModule.Users.Create", "R", "admin")]
    [InlineData("AbpIdentity.Users", "U", "550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("Permission.With.Many.Dots", "T", "tenant-key-123")]
    [InlineData("Simple", "R", "")]
    public void GetPermissionNameFormCacheKeyOrNull_Should_Extract_PermissionName(
        string permissionName, string providerName, string providerKey)
    {
        var key = PermissionGrantCacheItem.CalculateCacheKey(permissionName, providerName, providerKey);
        PermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key).ShouldBe(permissionName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("no-separator-here")]
    [InlineData("pn:R,pk:admin")]
    public void GetPermissionNameFormCacheKeyOrNull_Should_Return_Null_For_Invalid_Keys(string invalidKey)
    {
        PermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(invalidKey).ShouldBeNull();
    }
}
