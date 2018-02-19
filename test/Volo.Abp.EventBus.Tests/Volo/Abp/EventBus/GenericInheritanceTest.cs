using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Xunit;

namespace Volo.Abp.EventBus
{
    public class GenericInheritanceTest : EventBusTestBase
    {
        [Fact]
        public void Should_Trigger_For_Inherited_Generic_1()
        {
            var triggeredEvent = false;

            EventBus.Register<EntityChangedEventData<Person>>(
                eventData =>
                {
                    eventData.Entity.Id.ShouldBe(42);
                    triggeredEvent = true;
                });

            EventBus.Trigger(new EntityUpdatedEventData<Person>(new Person { Id = 42 }));

            triggeredEvent.ShouldBe(true);
        }

        [Fact]
        public void Should_Trigger_For_Inherited_Generic_2()
        {
            var triggeredEvent = false;

            EventBus.Register<EntityChangedEventData<Person>>(
                eventData =>
                {
                    eventData.Entity.Id.ShouldBe(42);
                    triggeredEvent = true;
                });

            EventBus.Trigger(new EntityChangedEventData<Student>(new Student { Id = 42 }));

            triggeredEvent.ShouldBe(true);
        }
        
        
        public class Person : Entity<int>
        {
            
        }

        public class Student : Person
        {
            
        }
    }
}