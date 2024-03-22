using System;
using System.Threading.Tasks;
using Volo.Abp.MultiQueue.Subscriber;
using Xunit;

namespace Volo.Abp.MultiQueue;
public class MultiQueueKafka_Tests : MultiQueueTestBase
{
    [Fact]
    public async Task Should_Pub_And_Receive()
    {
        var publisher = MultiQueueFactory.GetPublisher(MultiQueueTestConst.ConfigKey);
        var subscriber = MultiQueueFactory.GetSubscriber(MultiQueueTestConst.ConfigKey);

        await Task.Delay(10000); // 等待启动

        Assert.NotNull(publisher);
        Assert.NotNull(subscriber);

        var id = Guid.NewGuid();
        await publisher.PublishAsync(MultiQueueTestConst.Topic, new ReceiveEventData
        {
            Name = "Test",
            Id = id
        });

        await Task.Delay(8000);

        Assert.NotNull(JsonReceiveHandler.Result);

        Assert.Equal(1, JsonReceiveHandler.ReceiveCount);
        Assert.Equal(id, JsonReceiveHandler.Result.Id);
        Assert.Equal("Test", JsonReceiveHandler.Result.Name);

        var id2 = Guid.NewGuid();
        await publisher.PublishAsync(MultiQueueTestConst.Topic, new ReceiveEventData
        {
            Name = "Test2",
            Id = id2
        });

        await Task.Delay(8000);

        Assert.NotNull(JsonReceiveHandler.Result);
        Assert.Equal(2, JsonReceiveHandler.ReceiveCount);
        Assert.Equal(id2, JsonReceiveHandler.Result.Id);
        Assert.Equal("Test2", JsonReceiveHandler.Result.Name);

        await publisher.PublishAsync(MultiQueueTestConst.Topic, new ReceiveEventData
        {
            Name = "Test",
            Id = Guid.NewGuid()
        });
        await publisher.PublishAsync(MultiQueueTestConst.Topic, new ReceiveEventData
        {
            Name = "Test",
            Id = Guid.NewGuid()
        });
        await publisher.PublishAsync(MultiQueueTestConst.Topic, new ReceiveEventData
        {
            Name = "Test",
            Id = Guid.NewGuid()
        });

        await Task.Delay(8000);

        Assert.Equal(5, JsonReceiveHandler.ReceiveCount);
    }

}
