using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_Choreography_Test : EventBusTestBase
{
    private readonly ILocalEventBus _localEventBus;
    public EventBus_Choreography_Test()
    {
        _localEventBus = GetRequiredService<ILocalEventBus>();
    }

    [Fact]
    public async Task Should_Event_Choreography()
    {
        var eventData = new MyEventData { Result = "" };
        await _localEventBus.PublishAsync(eventData,false);
        eventData.Result.ShouldBe("123cache_handler");
    }

    public class MyEventData
    {
        public string Result { get; set; }
    }

    public class Handler1 : ILocalEventHandler<MyEventData>, ITransientDependency
    {
        public int Order { get; set; } = 1;

        public Task HandleEventAsync(MyEventData eventData)
        {
            eventData.Result += "1";
            return Task.CompletedTask;
        }
    }

    public class Handler2 : ILocalEventHandler<MyEventData>, ITransientDependency
    {
        public int Order { get; set; } = 2;

        public Task HandleEventAsync(MyEventData eventData)
        {
            eventData.Result += "2";
            return Task.CompletedTask;
        }
    }

    public class Handler3 : ILocalEventHandler<MyEventData>, ITransientDependency
    {
        public int Order { get; set; } = 3;

        public Task HandleEventAsync(MyEventData eventData)
        {
            eventData.Result += "3";
            return Task.CompletedTask;
        }
    }

    public class CacheHanlder : ILocalEventHandler<MyEventData>, ITransientDependency
    {
        public int Order { get; set; } = 3;

        public Task HandleEventAsync(MyEventData eventData)
        {
            eventData.Result += "cache_handler";
            return Task.CompletedTask;
        }
    }
}
