using System;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DomainEvents
{
    public class EntityChangeEvents_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IEventBus _eventBus;

        public EntityChangeEvents_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _eventBus = GetRequiredService<IEventBus>();
        }

        [Fact]
        public void Complex_Event_Test()
        {
            var personName = Guid.NewGuid().ToString("N");

            var creatingEventTriggered = false;
            var createdEventTriggered = false;
            var updatingEventTriggered = false;
            var updatedEventTriggered = false;

            _eventBus.Register<EntityCreatingEventData<Person>>(data =>
            {
                creatingEventTriggered.ShouldBeFalse();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeFalse();
                updatedEventTriggered.ShouldBeFalse();

                creatingEventTriggered = true;

                data.Entity.Name.ShouldBe(personName);

                /* Want to change age from 15 to 18
                 * Expect to trigger EntityUpdatingEventData, EntityUpdatedEventData events */
                data.Entity.Age.ShouldBe(15);
                data.Entity.Age = 18;
            });

            _eventBus.Register<EntityCreatedEventData<Person>>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeTrue();
                updatedEventTriggered.ShouldBeFalse();

                createdEventTriggered = true;

                data.Entity.Name.ShouldBe(personName);
            });

            _eventBus.Register<EntityUpdatingEventData<Person>>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeFalse();
                updatingEventTriggered.ShouldBeFalse();
                updatedEventTriggered.ShouldBeFalse();

                updatingEventTriggered = true;

                data.Entity.Name.ShouldBe(personName);
                data.Entity.Age.ShouldBe(18);
            });

            _eventBus.Register<EntityUpdatedEventData<Person>>(data =>
            {
                creatingEventTriggered.ShouldBeTrue();
                createdEventTriggered.ShouldBeTrue();
                updatingEventTriggered.ShouldBeTrue();
                updatedEventTriggered.ShouldBeFalse();

                updatedEventTriggered = true;

                data.Entity.Name.ShouldBe(personName);
                data.Entity.Age.ShouldBe(18);
            });

            _personRepository.Insert(new Person(Guid.NewGuid(), personName, 15));

            creatingEventTriggered.ShouldBeTrue();
            createdEventTriggered.ShouldBeTrue();
            updatingEventTriggered.ShouldBeTrue();
            updatedEventTriggered.ShouldBeTrue();
        }
    }
}
