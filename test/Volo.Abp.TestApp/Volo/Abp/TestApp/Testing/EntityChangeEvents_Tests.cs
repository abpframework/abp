using System;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class EntityChangeEvents_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IRepository<Person, Guid> PersonRepository { get; }
        protected IEventBus EventBus { get; }

        protected EntityChangeEvents_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            EventBus = GetRequiredService<IEventBus>();
        }

        [Fact]
        public void Complex_Event_Test()
        {
            var personName = Guid.NewGuid().ToString("N");

            var creatingEventTriggered = false;
            var createdEventTriggered = false;
            var updatingEventTriggered = false;
            var updatedEventTriggered = false;

            using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
            {
                EventBus.Register<EntityCreatingEventData<Person>>(data =>
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
                    PersonRepository.Update(data.Entity);
                });

                EventBus.Register<EntityCreatedEventData<Person>>(data =>
                {
                    creatingEventTriggered.ShouldBeTrue();
                    createdEventTriggered.ShouldBeFalse();
                    updatingEventTriggered.ShouldBeTrue();
                    updatedEventTriggered.ShouldBeFalse();

                    createdEventTriggered = true;

                    data.Entity.Age.ShouldBe(18);
                    data.Entity.Name.ShouldBe(personName);
                });

                EventBus.Register<EntityUpdatingEventData<Person>>(data =>
                {
                    creatingEventTriggered.ShouldBeTrue();
                    createdEventTriggered.ShouldBeFalse();
                    updatingEventTriggered.ShouldBeFalse();
                    updatedEventTriggered.ShouldBeFalse();

                    updatingEventTriggered = true;

                    data.Entity.Name.ShouldBe(personName);
                    data.Entity.Age.ShouldBe(18);
                });

                EventBus.Register<EntityUpdatedEventData<Person>>(data =>
                {
                    creatingEventTriggered.ShouldBeTrue();
                    createdEventTriggered.ShouldBeTrue();
                    updatingEventTriggered.ShouldBeTrue();
                    updatedEventTriggered.ShouldBeFalse();

                    updatedEventTriggered = true;

                    data.Entity.Name.ShouldBe(personName);
                    data.Entity.Age.ShouldBe(18);
                });

                PersonRepository.Insert(new Person(Guid.NewGuid(), personName, 15));

                uow.Complete();
            }
           
            creatingEventTriggered.ShouldBeTrue();
            createdEventTriggered.ShouldBeTrue();
            updatingEventTriggered.ShouldBeTrue();
            updatedEventTriggered.ShouldBeTrue();
        }
    }
}
