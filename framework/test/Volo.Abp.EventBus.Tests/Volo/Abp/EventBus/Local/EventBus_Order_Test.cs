using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_Order_Test : EventBusTestBase
{
    public static string HandlerExecuteOrder { get; set; }

    [Fact]
    public async Task Handler_Should_Execute_By_Order()
    {
        HandlerExecuteOrder = "";
        await LocalEventBus.PublishAsync(new MyOrderEventHandlerEventData());
        HandlerExecuteOrder.ShouldBe("321");
    }

    public class MyOrderEventHandlerEventData
    {

    }

    public class MyOrderEventHandler : ILocalEventHandler<MyOrderEventHandlerEventData>, ITransientDependency
    {
        public Task HandleEventAsync(MyOrderEventHandlerEventData eventData)
        {
            HandlerExecuteOrder += "1";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-2)]
    public class MyOrderEventHandler2 : ILocalEventHandler<MyOrderEventHandlerEventData>, ITransientDependency
    {
        public Task HandleEventAsync(MyOrderEventHandlerEventData eventData)
        {
            HandlerExecuteOrder += "2";
            return Task.CompletedTask;
        }
    }

    [LocalEventHandlerOrder(-3)]
    public class MyOrderEventHandler3 : ILocalEventHandler<MyOrderEventHandlerEventData>, ITransientDependency
    {
        public Task HandleEventAsync(MyOrderEventHandlerEventData eventData)
        {
            HandlerExecuteOrder += "3";
            return Task.CompletedTask;
        }
    }

}
