using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class GenericEventNameAttribute_Tests
    {
        [Fact]
        public void Should_Properly_Get_EventName()
        {
            new GenericEventNameAttribute()
                .GetName(typeof(MyGenericType<MyInnerType>))
                .ShouldBe(typeof(MyInnerType).FullName);
        }

        public class MyInnerType
        {

        }

        public class MyGenericType<T>
        {

        }
    }
}
