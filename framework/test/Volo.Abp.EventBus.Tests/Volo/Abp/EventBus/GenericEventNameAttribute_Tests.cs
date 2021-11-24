using System.Linq;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus;

public class GenericEventNameAttribute_Tests
{
    [Fact]
    public void Should_Properly_Get_EventName()
    {
        var eventType = typeof(MyGenericType<MyInnerType>);

        var eventNameProvider = eventType
            .GetCustomAttributes(true)
            .OfType<IEventNameProvider>()
            .Single();

        eventNameProvider
            .GetName(eventType)
            .ShouldBe("MyEvent.GenericTest");
    }

    [EventName("MyEvent")]
    public class MyInnerType
    {

    }

    [GenericEventName(Postfix = ".GenericTest")]
    public class MyGenericType<T>
    {

    }
}
