using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class EntityChangeEvents_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IRepository<Person, Guid> PersonRepository { get; }
    protected ILocalEventBus LocalEventBus { get; }
    protected IDistributedEventBus DistributedEventBus { get; }

    protected EntityChangeEvents_Tests()
    {
        PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
        LocalEventBus = GetRequiredService<ILocalEventBus>();
        DistributedEventBus = GetRequiredService<IDistributedEventBus>();
    }

    [Fact]
    public async Task Complex_Event_Test()
    {
        var personName = Guid.NewGuid().ToString("N");

        var createdEventTriggered = false;
        var createdEtoTriggered = false;

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            LocalEventBus.Subscribe<EntityCreatedEventData<Person>>(data =>
            {
                createdEventTriggered.ShouldBeFalse();

                createdEventTriggered = true;

                data.Entity.Age.ShouldBe(15);
                data.Entity.Name.ShouldBe(personName);

                return Task.CompletedTask;
            });

            DistributedEventBus.Subscribe<EntityCreatedEto<PersonEto>>(eto =>
            {
                eto.Entity.Name.ShouldBe(personName);

                createdEtoTriggered = true;

                return Task.CompletedTask;
            });

            await PersonRepository.InsertAsync(new Person(Guid.NewGuid(), personName, 15));

            await uow.CompleteAsync();
        }
        
        createdEventTriggered.ShouldBeTrue();
        createdEtoTriggered.ShouldBeTrue();
    }

    [Fact]
    public async Task Multiple_Update_Should_Result_With_Single_Updated_Event_In_The_Same_Uow()
    {
        var createEventCount = 0;
        var updateEventCount = 0;
        var updatedAge = 0;

        DistributedEventBus.Subscribe<EntityCreatedEto<PersonEto>>(eto =>
        {
            createEventCount++;
            return Task.CompletedTask;
        });

        DistributedEventBus.Subscribe<EntityUpdatedEto<PersonEto>>(eto =>
        {
            updateEventCount++;
            updatedAge = eto.Entity.Age;
            return Task.CompletedTask;
        });

        var personId = Guid.NewGuid();
        await PersonRepository.InsertAsync(new Person(personId, Guid.NewGuid().ToString("D"), 42));

        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var person = await PersonRepository.GetAsync(personId);

            person.Age = 43;
            await PersonRepository.UpdateAsync(person, autoSave: true);
            updateEventCount.ShouldBe(0);

            person.Age = 44;
            await PersonRepository.UpdateAsync(person, autoSave: true);
            updateEventCount.ShouldBe(0);

            person.Age = 45;
            await PersonRepository.UpdateAsync(person, autoSave: true);
            updateEventCount.ShouldBe(0);

            await uow.CompleteAsync();
        }

        createEventCount.ShouldBe(1);
        updateEventCount.ShouldBe(1);
        updatedAge.ShouldBe(45);
    }
}
