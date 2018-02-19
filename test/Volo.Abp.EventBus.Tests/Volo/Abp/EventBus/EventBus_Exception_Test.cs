using System;
using Shouldly;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class EventBus_Exception_Test : EventBusTestBase
    {
        [Fact]
        public void Should_Throw_Single_Exception_If_Only_One_Of_Handlers_Fails()
        {
            EventBus.Register<MySimpleEventData>(
                eventData => throw new Exception("This exception is intentionally thrown!"));

            var appException = Assert.Throws<Exception>(() =>
            {
                EventBus.Trigger(new MySimpleEventData(1));
            });

            appException.Message.ShouldBe("This exception is intentionally thrown!");
        }

        [Fact]
        public void Should_Throw_Aggregate_Exception_If_More_Than_One_Of_Handlers_Fail()
        {
            EventBus.Register<MySimpleEventData>(
                eventData => throw new Exception("This exception is intentionally thrown #1!"));

            EventBus.Register<MySimpleEventData>(
                eventData => throw new Exception("This exception is intentionally thrown #2!"));

            var aggrException = Assert.Throws<AggregateException>(() =>
            {
                EventBus.Trigger(new MySimpleEventData(1));
            });

            aggrException.InnerExceptions.Count.ShouldBe(2);
            aggrException.InnerExceptions[0].Message.ShouldBe("This exception is intentionally thrown #1!");
            aggrException.InnerExceptions[1].Message.ShouldBe("This exception is intentionally thrown #2!");
        }
    }
}