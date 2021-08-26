using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class DomainEvents_Tests<TStartupModule> : TestAppTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;
        protected readonly ILocalEventBus LocalEventBus;
        protected readonly IDistributedEventBus DistributedEventBus;

        protected DomainEvents_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            LocalEventBus = GetRequiredService<ILocalEventBus>();
            DistributedEventBus = GetRequiredService<IDistributedEventBus>();
        }

        [Fact]
        public virtual async Task Should_Rollback_Uow_If_Event_Handler_Throws_Exception()
        {
            (await PersonRepository.FindAsync(x => x.Name == "TestPerson1")).ShouldBeNull();
            
            LocalEventBus.Subscribe<EntityCreatedEventData<Person>>(data =>
            {
                data.Entity.Name.ShouldBe("TestPerson1");
                throw new ApplicationException("Just to rollback the UOW");
            });

            var exception = await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await WithUnitOfWorkAsync(new AbpUnitOfWorkOptions{IsTransactional = true}, async () =>
                {
                    await PersonRepository.InsertAsync(
                        new Person(Guid.NewGuid(), "TestPerson1", 42)
                    );
                });
            });
            
            exception.Message.ShouldBe("Just to rollback the UOW");

            (await PersonRepository.FindAsync(x => x.Name == "TestPerson1")).ShouldBeNull();
        }

        [Fact]
        public async Task Should_Trigger_Domain_Events_For_Aggregate_Root()
        {
            //Arrange

            var isLocalEventTriggered = false;
            var isDistributedEventTriggered = false;

            LocalEventBus.Subscribe<PersonNameChangedEvent>(data =>
            {
                data.OldName.ShouldBe("Douglas");
                data.Person.Name.ShouldBe("Douglas-Changed");
                isLocalEventTriggered = true;
                return Task.CompletedTask;
            });

            DistributedEventBus.Subscribe<PersonNameChangedEto>(data =>
            {
                data.OldName.ShouldBe("Douglas");
                data.NewName.ShouldBe("Douglas-Changed");
                isDistributedEventTriggered = true;
                return Task.CompletedTask;
            });

            //Act

            await WithUnitOfWorkAsync(async () =>
            {
                var dougles = PersonRepository.Single(b => b.Name == "Douglas");
                dougles.ChangeName("Douglas-Changed");
                await PersonRepository.UpdateAsync(dougles);
            });

            //Assert

            isLocalEventTriggered.ShouldBeTrue();
            isDistributedEventTriggered.ShouldBeTrue();
        }
    }
}