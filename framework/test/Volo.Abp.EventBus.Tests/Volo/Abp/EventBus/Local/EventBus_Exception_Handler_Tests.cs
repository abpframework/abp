using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus.Local
{
    public class EventBus_Exception_Handler_Tests : EventBusTestBase
    {
        [Fact]
        public async Task Should_Not_Handle_Exception()
        {
            MySimpleEventData data = null;
            LocalEventBus.Subscribe<MySimpleEventData>(eventData =>
            {
                ++eventData.Value;
                data = eventData;
                throw new Exception("This exception is intentionally thrown!");
            });

            var appException = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await LocalEventBus.PublishAsync(new MySimpleEventData(1));
            });

            data.Value.ShouldBe(2);
            appException.Message.ShouldBe("This exception is intentionally thrown!");
        }

        [Fact]
        public async Task Should_Handle_Exception()
        {
            MyExceptionHandleEventData data = null;
            LocalEventBus.Subscribe<MyExceptionHandleEventData>(eventData =>
            {
                ++eventData.RetryAttempts;
                data = eventData;

                if (eventData.RetryAttempts < 2)
                {
                    throw new Exception("This exception is intentionally thrown!");
                }

                return Task.CompletedTask;

            });

            await LocalEventBus.PublishAsync(new MyExceptionHandleEventData(0));
            data.RetryAttempts.ShouldBe(2);
        }

        [Fact]
        public async Task Should_Throw_Exception_After_Error_Handle()
        {
            MyExceptionHandleEventData data = null;
            LocalEventBus.Subscribe<MyExceptionHandleEventData>(eventData =>
            {
                ++eventData.RetryAttempts;
                data = eventData;
                throw new Exception("This exception is intentionally thrown!");
            });

            var appException = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await LocalEventBus.PublishAsync(new MyExceptionHandleEventData(0));
            });

            data.RetryAttempts.ShouldBe(4);
            appException.Message.ShouldBe("This exception is intentionally thrown!");
        }
    }
}
