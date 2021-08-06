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
            var retryAttempt = 0;
            LocalEventBus.Subscribe<MySimpleEventData>(eventData =>
            {
                retryAttempt++;
                throw new Exception("This exception is intentionally thrown!");
            });

            var appException = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await LocalEventBus.PublishAsync(new MySimpleEventData(1));
            });

            retryAttempt.ShouldBe(1);
            appException.Message.ShouldBe("This exception is intentionally thrown!");
        }

        [Fact]
        public async Task Should_Handle_Exception()
        {
            var retryAttempt = 0;
            LocalEventBus.Subscribe<MyExceptionHandleEventData>(eventData =>
            {
                eventData.Value.ShouldBe(0);
                retryAttempt++;
                if (retryAttempt < 2)
                {
                    throw new Exception("This exception is intentionally thrown!");
                }

                return Task.CompletedTask;

            });

            await LocalEventBus.PublishAsync(new MyExceptionHandleEventData(0));
            retryAttempt.ShouldBe(2);
        }

        [Fact]
        public async Task Should_Throw_Exception_After_Error_Handle()
        {
            var retryAttempt = 0;
            LocalEventBus.Subscribe<MyExceptionHandleEventData>(eventData =>
            {
                eventData.Value.ShouldBe(0);

                retryAttempt++;

                throw new Exception("This exception is intentionally thrown!");
            });

            var appException = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await LocalEventBus.PublishAsync(new MyExceptionHandleEventData(0));
            });

            retryAttempt.ShouldBe(4);
            appException.Message.ShouldBe("This exception is intentionally thrown!");
        }
    }
}
