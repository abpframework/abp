using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class ActionBasedEventHandlerTest : EventBusTestBase
    {
        [Fact]
        public async Task Should_Call_Action_On_Event_With_Correct_Source()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await EventBus.TriggerAsync(new MySimpleEventData(1));
            await EventBus.TriggerAsync(new MySimpleEventData(2));
            await EventBus.TriggerAsync(new MySimpleEventData(3));
            await EventBus.TriggerAsync(new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public async Task Should_Call_Handler_With_Non_Generic_Trigger()
        {
            var totalData = 0;

            EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await EventBus.TriggerAsync(typeof(MySimpleEventData), new MySimpleEventData(1));
            await EventBus.TriggerAsync(typeof(MySimpleEventData), new MySimpleEventData(2));
            await EventBus.TriggerAsync(typeof(MySimpleEventData), new MySimpleEventData(3));
            await EventBus.TriggerAsync(typeof(MySimpleEventData), new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }

        [Fact]
        public async Task Should_Not_Call_Action_After_Unregister_1()
        {
            var totalData = 0;

            var registerDisposer = EventBus.Register<MySimpleEventData>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            await EventBus.TriggerAsync(new MySimpleEventData(1));
            await EventBus.TriggerAsync(new MySimpleEventData(2));
            await EventBus.TriggerAsync(new MySimpleEventData(3));

            registerDisposer.Dispose();

            await EventBus.TriggerAsync(new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }

        [Fact]
        public async Task Should_Not_Call_Action_After_Unregister_2()
        {
            var totalData = 0;

            var action = new Func<MySimpleEventData, Task>(
                eventData =>
                {
                    totalData += eventData.Value;
                    return Task.CompletedTask;
                });

            EventBus.Register(action);

            await EventBus.TriggerAsync(new MySimpleEventData(1));
            await EventBus.TriggerAsync(new MySimpleEventData(2));
            await EventBus.TriggerAsync(new MySimpleEventData(3));

            EventBus.Unregister(action);

            await EventBus.TriggerAsync(new MySimpleEventData(4));

            Assert.Equal(6, totalData);
        }

        [Fact]
        public async Task Should_Call_Action_On_Event_With_Correct_Source_Async()
        {
            int totalData = 0;

            EventBus.Register<MySimpleEventData>(
                async eventData =>
                {
                    await Task.Delay(20);
                    Interlocked.Add(ref totalData, eventData.Value);
                    await Task.Delay(20);
                });

            await EventBus.TriggerAsync(new MySimpleEventData(1));
            await EventBus.TriggerAsync(new MySimpleEventData(2));
            await EventBus.TriggerAsync(new MySimpleEventData(3));
            await EventBus.TriggerAsync(new MySimpleEventData(4));

            Assert.Equal(10, totalData);
        }
    }
}
