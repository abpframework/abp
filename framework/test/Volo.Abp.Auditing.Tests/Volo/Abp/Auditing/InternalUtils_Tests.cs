using Shouldly;
using Xunit;

namespace Volo.Abp.Auditing
{
    public static class InternalUtils_Tests
    {
        [Fact]
        public static void AddCounter()
        {
            InternalUtils.AddCounter("test").ShouldBe("test__2");
            InternalUtils.AddCounter("test__2").ShouldBe("test__3");
            InternalUtils.AddCounter("test__a").ShouldBe("test__a__2");
        }
    }
}
