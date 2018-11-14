using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class DomainEvents_Tests<TStartupModule> : TestAppTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected readonly IRepository<Person, Guid> PersonRepository;
        protected readonly ILocalEventBus LocalEventBus;

        protected DomainEvents_Tests()
        {
            PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
            LocalEventBus = GetRequiredService<ILocalEventBus>();
        }

        [Fact]
        public async Task Should_Trigger_Domain_Events_For_Aggregate_Root()
        {
            //Arrange

            var isTriggered = false;

            LocalEventBus.Subscribe<PersonNameChangedEvent>(data =>
            {
                data.OldName.ShouldBe("Douglas");
                data.Person.Name.ShouldBe("Douglas-Changed");
                isTriggered = true;
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

            isTriggered.ShouldBeTrue();
        }
    }
}