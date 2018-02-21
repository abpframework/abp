using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore
{
    public class DomainEvents_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IEventBus _eventBus;

        public DomainEvents_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _eventBus = GetRequiredService<IEventBus>();
        }

        [Fact]
        public async Task Should_Trigger_Domain_Events_For_Aggregate_Root()
        {
            //Arrange

            var isTriggered = false;

            _eventBus.Register<PersonNameChangedEvent>((data) =>
            {
                data.OldName.ShouldBe("Douglas");
                data.Person.Name.ShouldBe("Douglas-Changed");
                isTriggered = true;
            });

            //Act

            await WithUnitOfWorkAsync(async () =>
            {
                var dougles = await _personRepository.SingleAsync(b => b.Name == "Douglas");
                dougles.ChangeName("Douglas-Changed");
                await _personRepository.UpdateAsync(dougles);
            });

            //Assert

            isTriggered.ShouldBeTrue();
        }
    }
}