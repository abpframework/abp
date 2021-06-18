using Shouldly;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public class SettingCacheItem_Tests
    {
        [Fact]
        public void GetSettingNameFormCacheKeyOrNull()
        {
            var key = SettingCacheItem.CalculateCacheKey("aaa", "bbb", "ccc");
            SettingCacheItem.GetSettingNameFormCacheKeyOrNull(key).ShouldBe("aaa");
            SettingCacheItem.GetSettingNameFormCacheKeyOrNull("aaabbbccc").ShouldBeNull();
        }
    }
}
