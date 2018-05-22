using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class EventBus_EntityEvents_Test : EventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Created_And_Changed_Once()
        {
            var handler = new MyEventHandler();

            EventBus.Register<EntityChangedEventData<MyEntity>>(handler);
            EventBus.Register<EntityCreatedEventData<MyEntity>>(handler);

            var asyncHandler = new MyAsyncEventHandler();

            EventBus.Register<EntityChangedEventData<MyEntity>>(asyncHandler);
            EventBus.Register<EntityCreatedEventData<MyEntity>>(asyncHandler);

            await EventBus.TriggerAsync(new EntityCreatedEventData<MyEntity>(new MyEntity()));

            handler.EntityCreatedEventCount.ShouldBe(1);
            handler.EntityChangedEventCount.ShouldBe(1);

            asyncHandler.EntityCreatedEventCount.ShouldBe(1);
            asyncHandler.EntityChangedEventCount.ShouldBe(1);
        }

        public class MyEntity : Entity
        {
            
        }

        public class MyEventHandler :
            IAsyncEventHandler<EntityChangedEventData<MyEntity>>,
            IAsyncEventHandler<EntityCreatedEventData<MyEntity>>
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

        public class MyAsyncEventHandler :
            IAsyncEventHandler<EntityChangedEventData<MyEntity>>,
            IAsyncEventHandler<EntityCreatedEventData<MyEntity>>
        {
            public int EntityChangedEventCount { get; set; }
            public int EntityCreatedEventCount { get; set; }

            public Task HandleEventAsync(EntityChangedEventData<MyEntity> eventData)
            {
                EntityChangedEventCount++;
                return Task.FromResult(0);
            }

            public Task HandleEventAsync(EntityCreatedEventData<MyEntity> eventData)
            {
                EntityCreatedEventCount++;
                return Task.FromResult(0);
            }
        }
    }
}
