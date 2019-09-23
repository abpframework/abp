using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Xunit;

namespace Volo.Abp.EventBus.Local
{
    public class GenericInheritanceTest : EventBusTestBase
    {
        [Fact]
        public async Task Should_Trigger_For_Inherited_Generic_1()
        {
            var triggeredEvent = false;

            LocalEventBus.Subscribe<EntityChangedEventData<Person>>(
                eventData =>
                {
                    eventData.Entity.Id.ShouldBe(42);
                    triggeredEvent = true;
                    return Task.CompletedTask;
                });

            await LocalEventBus.PublishAsync(new EntityUpdatedEventData<Person>(new Person(42)));

            triggeredEvent.ShouldBe(true);
        }

        [Fact]
        public async Task Should_Trigger_For_Inherited_Generic_2()
        {
            var triggeredEvent = false;

            LocalEventBus.Subscribe<EntityChangedEventData<Person>>(
                eventData =>
                {
                    eventData.Entity.Id.ShouldBe(42);
                    triggeredEvent = true;
                    return Task.CompletedTask;
                });

            await LocalEventBus.PublishAsync(new EntityChangedEventData<Student>(new Student(42)));

            triggeredEvent.ShouldBe(true);
        }

        public class Person : Entity<int>
        {
            public Person()
            {

            }

            public Person(int id)
             : base(id)
            {
                
            }
        }

        public class Student : Person
        {
            public Student()
            {

            }

            public Student(int id)
            : base(id)
            {

            }
        }
    }
}