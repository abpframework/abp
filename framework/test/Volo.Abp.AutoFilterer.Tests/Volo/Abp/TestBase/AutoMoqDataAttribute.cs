using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Volo.Abp.AutoFilterer.Tests.Volo.Abp.TestBase
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute(int count = 3)
           : base(() => new Fixture { RepeatCount = count, }.Customize(new AutoMoqCustomization()))
        {
        }
    }
}
