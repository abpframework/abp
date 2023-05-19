using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Xunit;

namespace Volo.Abp.EventBus.Local;

public class EventBus_EntityEvents_Test : EventBusTestBase
{
    [Fact]
    public async Task Should_Call_Created_And_Changed_Once()
    {
        var handler = new MyEventHandler();

        LocalEventBus.Subscribe<EntityChangedEventData<MyEntity>>(handler);
        LocalEventBus.Subscribe<EntityCreatedEventData<MyEntity>>(handler);

        await LocalEventBus.PublishAsync(new EntityCreatedEventData<MyEntity>(new MyEntity()));

        handler.EntityCreatedEventCount.ShouldBe(1);
        handler.EntityChangedEventCount.ShouldBe(1);
    }

    public class MyEntity : Entity
    {
        public override object[] GetKeys()
        {
            return new object[0];
        }
    }

    public class MyEventHandler :
        ILocalEventHandler<EntityChangedEventData<MyEntity>>,
        ILocalEventHandler<EntityCreatedEventData<MyEntity>>
    {
        public int EntityChangedEventCount { get; set; }
        public int EntityCreatedEventCount { get; set; }

        public Task HandleEventAsync(EntityChangedEventData<MyEntity> eventData)
        {
            EntityChangedEventCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityCreatedEventData<MyEntity> eventData)
        {
            EntityCreatedEventCount++;
            return Task.CompletedTask;
        }
    }
}
