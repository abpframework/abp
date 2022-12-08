using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Xunit;

namespace Volo.Abp.EventBus;

public class EventHandlerInvoker_Tests : EventBusTestBase
{
    private readonly IEventHandlerInvoker _eventHandlerInvoker;

    public EventHandlerInvoker_Tests()
    {
        _eventHandlerInvoker = GetRequiredService<IEventHandlerInvoker>();
    }

    [Fact]
    public async Task Should_Invoke_LocalEventHandler_With_MyEventData()
    {
        var localHandler = new MyLocalEventHandler();
        var eventData = new MyEventData();

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());

        localHandler.MyEventDataCount.ShouldBe(2);
        localHandler.EntityChangedEventDataCount.ShouldBe(0);
        localHandler.EntityChangedEventDataCount.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Invoke_LocalEventHandler_Created_And_Changed_Once()
    {
        var localHandler = new MyLocalEventHandler();
        var eventData = new EntityCreatedEventData<MyEntity>(new MyEntity());

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());
        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, typeof(EntityChangedEventData<MyEntity>));

        localHandler.MyEventDataCount.ShouldBe(0);
        localHandler.EntityChangedEventDataCount.ShouldBe(1);
        localHandler.EntityChangedEventDataCount.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Invoke_DistributedEventHandler_With_MyEventData()
    {
        var localHandler = new MyDistributedEventHandler();
        var eventData = new MyEventData();

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());

        localHandler.MyEventDataCount.ShouldBe(1);
        localHandler.EntityCreatedCount.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Invoke_DistributedEventHandler_With_EntityCreatedEto()
    {
        var localHandler = new MyDistributedEventHandler();
        var eventData = new EntityCreatedEto<MyEntity>(new MyEntity());

        await _eventHandlerInvoker.InvokeAsync(localHandler, eventData, eventData.GetType());

        localHandler.MyEventDataCount.ShouldBe(0);
        localHandler.EntityCreatedCount.ShouldBe(1);
    }

    public class MyEventData
    {
    }

    public class MyEntity : Entity<Guid>
    {

    }

    public class MyDistributedEventHandler : IDistributedEventHandler<MyEventData>,
        IDistributedEventHandler<EntityCreatedEto<MyEntity>>
    {
        public int MyEventDataCount { get; set; }
        public int EntityCreatedCount { get; set; }

        public Task HandleEventAsync(MyEventData eventData)
        {
            MyEventDataCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityCreatedEto<MyEntity> eventData)
        {
            EntityCreatedCount++;
            return Task.CompletedTask;
        }
    }

    public class MyLocalEventHandler : ILocalEventHandler<MyEventData>,
        IDistributedEventHandler<MyEventData>,
        IDistributedEventHandler<EntityCreatedEventData<MyEntity>>,
        IDistributedEventHandler<EntityChangedEventData<MyEntity>>
    {
        public int MyEventDataCount { get; set; }
        public int EntityCreatedEventDataCount { get; set; }
        public int EntityChangedEventDataCount { get; set; }

        public Task HandleEventAsync(MyEventData eventData)
        {
            MyEventDataCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityCreatedEventData<MyEntity> eventData)
        {
            EntityCreatedEventDataCount++;
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(EntityChangedEventData<MyEntity> eventData)
        {
            EntityChangedEventDataCount++;
            return Task.CompletedTask;
        }
    }
}
