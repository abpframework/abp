using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DomainEvents;

public class DomainEvents_Tests : DomainEvents_Tests<AbpEntityFrameworkCoreTestModule>
{
}

public class AbpEntityChangeOptions_DomainEvents_PublishEntityUpdatedEventWhenNavigationChanges_Tests : AbpEntityChangeOptions_DomainEvents_Tests<AbpEntityFrameworkCoreTestModule>
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityChangeOptions>(options =>
        {
            options.PublishEntityUpdatedEventWhenNavigationChanges = false;
        });

        base.AfterAddApplication(services);
    }
}

public class AbpEntityChangeOptions_DomainEvents_IgnoreEntityChangeSelectorList_Tests : AbpEntityChangeOptions_DomainEvents_Tests<AbpEntityFrameworkCoreTestModule>
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityChangeOptions>(options =>
        {
            options.PublishEntityUpdatedEventWhenNavigationChanges = true;
            options.IgnoredNavigationEntitySelectors.Add("DisableAllEntity", _ => true);
        });

        base.AfterAddApplication(services);
    }
}

public class AbpEfCoreDomainEvents_Tests : EntityFrameworkCoreTestBase
{
    protected readonly IRepository<AppEntityWithNavigations, Guid> AppEntityWithNavigationsRepository;
    protected readonly IRepository<AppEntityWithNavigationChildOneToMany,  Guid> AppEntityWithNavigationChildOneToManyRepository;
    protected readonly ILocalEventBus LocalEventBus;
    protected readonly IRepository<Person, Guid> PersonRepository;
    protected bool _loadEntityWithoutDetails = false;

    public AbpEfCoreDomainEvents_Tests()
    {
        AppEntityWithNavigationsRepository = GetRequiredService<IRepository<AppEntityWithNavigations, Guid>>();
        AppEntityWithNavigationChildOneToManyRepository = GetRequiredService<IRepository<AppEntityWithNavigationChildOneToMany, Guid>>();
        LocalEventBus = GetRequiredService<ILocalEventBus>();
        PersonRepository = GetRequiredService<IRepository<Person, Guid>>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityOptions>(options =>
        {
            options.Entity<AppEntityWithNavigations>(opt =>
            {
                if (_loadEntityWithoutDetails)
                {
                    opt.DefaultWithDetailsFunc = q => q;
                }
            });
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task Should_Trigger_Domain_Events_For_Aggregate_Root_When_Navigation_Changes_Tests()
    {
        _loadEntityWithoutDetails = false;

        var entityId = Guid.NewGuid();

        await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity"));

        var entityUpdatedEventTriggered = false;
        var personCreatedEventCount = 0;
        var entityUpdatedEventTriggerCount = 0;

        LocalEventBus.Subscribe<EntityCreatedEventData<Person>>(_ =>
        {
            personCreatedEventCount++;
           return Task.CompletedTask;
        });

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigations>>(async _ =>
        {
            entityUpdatedEventTriggered = true;
            await PersonRepository.InsertAsync(new Person(Guid.NewGuid(), Guid.NewGuid().ToString(), new Random().Next(1, 100)));
        });

        var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        // Test with simple property
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.Name = Guid.NewGuid().ToString();
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        // Test with value object
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.AppEntityWithValueObjectAddress = new AppEntityWithValueObjectAddress("Turkey");
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.AppEntityWithValueObjectAddress.Country = "USA";
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithValueObjectAddress>>(_ =>
        {
            throw new Exception("Should not trigger this event");
        });

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.AppEntityWithValueObjectAddress = null;
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        // Test with one to one
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToOne = new AppEntityWithNavigationChildOneToOne
            {
                ChildName = "ChildName",
                OneToOne = new AppEntityWithNavigationChildOneToOneAndOneToOne
                {
                    ChildName = "OneToOne-ChildName"
                }
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        var oneToOneEntityUpdatedEventTriggered = false;
        var oneToOneAndOneToOneEntityUpdatedEventTriggered = false;

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToOne>>(async _ =>
        {
            oneToOneEntityUpdatedEventTriggered = true;
        });
        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToOneAndOneToOne>>(async _ =>
        {
            oneToOneAndOneToOneEntityUpdatedEventTriggered = true;
        });

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToOne.ChildName = "ChildName2";
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        oneToOneEntityUpdatedEventTriggered.ShouldBeTrue();
        oneToOneAndOneToOneEntityUpdatedEventTriggered.ShouldBeFalse();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        entityUpdatedEventTriggered = false;
        oneToOneEntityUpdatedEventTriggered = false;
        oneToOneAndOneToOneEntityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToOne.OneToOne.ChildName = "OneToOne-ChildName2";
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        oneToOneEntityUpdatedEventTriggered.ShouldBeTrue();
        oneToOneAndOneToOneEntityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToOne>>(_ =>
        {
            throw new Exception("Should not trigger this event");
        });

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToOne = null;
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        // Test with one to many
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToMany = new List<AppEntityWithNavigationChildOneToMany>()
            {
                new AppEntityWithNavigationChildOneToMany
                {
                    AppEntityWithNavigationId = entity.Id,
                    ChildName = "ChildName1",
                    OneToMany = new List<AppEntityWithNavigationChildOneToManyAndOneToMany>()
                    {
                        new AppEntityWithNavigationChildOneToManyAndOneToMany()
                        {
                            ChildName = "OneToMany-ChildName1"
                        }
                    }
                }
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        var oneToManyEntityUpdatedEventTriggered = false;
        var oneToManyAndOneToManyEntityUpdatedEventTriggered = false;

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToMany>>(async _ =>
        {
            oneToManyEntityUpdatedEventTriggered = true;
        });
        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToManyAndOneToMany>>(async _ =>
        {
            oneToManyAndOneToManyEntityUpdatedEventTriggered = true;
        });

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToMany[0].ChildName = "ChildName2";
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        oneToManyEntityUpdatedEventTriggered.ShouldBeTrue();
        oneToManyAndOneToManyEntityUpdatedEventTriggered.ShouldBeFalse();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        entityUpdatedEventTriggered = false;
        oneToManyEntityUpdatedEventTriggered = false;
        oneToManyAndOneToManyEntityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToMany[0].OneToMany[0].ChildName = "OneToMany-ChildName2";
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        oneToManyEntityUpdatedEventTriggered.ShouldBeTrue();
        oneToManyAndOneToManyEntityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToMany>>(_ =>
        {
            throw new Exception("Should not trigger this event");
        });

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.OneToMany.Clear();
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        // Test with many to many
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.ManyToMany = new List<AppEntityWithNavigationChildManyToMany>()
            {
                new AppEntityWithNavigationChildManyToMany
                {
                    ChildName = "ChildName1"
                }
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.ManyToMany[0].ChildName = "ChildName2";
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);

        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            await unitOfWorkManager.Current.SaveChangesAsync();
            entity.ManyToMany.Clear();
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
        personCreatedEventCount.ShouldBe(++entityUpdatedEventTriggerCount);
    }

    [Fact]
    public async Task Should_Trigger_Domain_Events_For_Aggregate_Root_When_EnsureCollectionLoaded_Navigation_Changes_Tests()
    {
        _loadEntityWithoutDetails = true;

        var entityId = Guid.NewGuid();

        await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity")
        {
            OneToMany = new List<AppEntityWithNavigationChildOneToMany>()
            {
                new AppEntityWithNavigationChildOneToMany(Guid.NewGuid())
                {
                    ChildName = "ChildName1"
                },
                new AppEntityWithNavigationChildOneToMany(Guid.NewGuid())
                {
                    ChildName = "ChildName2"
                }
            }
        });

        var entityId2 = Guid.NewGuid();
        await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId2, "TestEntity")
        {
            OneToMany = new List<AppEntityWithNavigationChildOneToMany>()
            {
                new AppEntityWithNavigationChildOneToMany
                {
                    ChildName = "ChildName1"
                }
            }
        });

        var oneToManyEntity = Guid.NewGuid();
        await AppEntityWithNavigationChildOneToManyRepository.InsertAsync(
            new AppEntityWithNavigationChildOneToMany(oneToManyEntity)
            {
                AppEntityWithNavigationId = entityId,
            });

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToMany>>(data =>
        {
            data.Entity.AppEntityWithNavigationId.ShouldBe(entityId2);
            return Task.CompletedTask;
        });

        using (var scope = ServiceProvider.CreateScope())
        {
            var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            using (var uow = uowManager.Begin())
            {
                var entity = await AppEntityWithNavigationChildOneToManyRepository.GetAsync(oneToManyEntity);

                entity.AppEntityWithNavigationId = entityId2;
                await AppEntityWithNavigationChildOneToManyRepository.UpdateAsync(entity);

                await uow.CompleteAsync();
            }
        }

        var entityUpdatedEventTriggered = false;

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigations>>(data =>
        {
            entityUpdatedEventTriggered = !entityUpdatedEventTriggered;
            return Task.CompletedTask;
        });

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationChildOneToMany>>(data =>
        {
            throw new Exception("Should not trigger this event");
        });

        using (var scope = ServiceProvider.CreateScope())
        {
            var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            using (var uow = uowManager.Begin())
            {
                var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
                entity.OneToMany.ShouldBeNull();

                await AppEntityWithNavigationsRepository.EnsureCollectionLoadedAsync(entity, x => x.OneToMany);
                entity.OneToMany.ShouldNotBeNull();

                entity.OneToMany.Clear();
                await AppEntityWithNavigationsRepository.UpdateAsync(entity);

                await uow.CompleteAsync();
            }
        }

        entityUpdatedEventTriggered.ShouldBeTrue();
    }
}


public abstract class AbpEfCoreDomainEvents_Disable_UpdateAggregateRoot_Tests : EntityFrameworkCoreTestBase
{
    protected readonly IRepository<AppEntityWithNavigations, Guid> AppEntityWithNavigationsRepository;
    protected readonly IRepository<AppEntityWithNavigationsForeign, Guid> AppEntityWithNavigationForeignRepository;
    protected readonly ILocalEventBus LocalEventBus;

    protected AbpEfCoreDomainEvents_Disable_UpdateAggregateRoot_Tests()
    {
        AppEntityWithNavigationsRepository = GetRequiredService<IRepository<AppEntityWithNavigations, Guid>>();
        AppEntityWithNavigationForeignRepository = GetRequiredService<IRepository<AppEntityWithNavigationsForeign, Guid>>();
        LocalEventBus = GetRequiredService<ILocalEventBus>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityChangeOptions>(options =>
        {
            options.PublishEntityUpdatedEventWhenNavigationChanges = true;
            options.UpdateAggregateRootWhenNavigationChanges = false;
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task Should_Trigger_Domain_Events_But_Do_Not_Change_Aggregate_Root_When_Navigation_Changes_Tests()
    {
        var entityId = Guid.NewGuid();

        var newEntity = await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity"));

        var latestConcurrencyStamp = newEntity.ConcurrencyStamp;
        var lastModificationTime = newEntity.LastModificationTime;

        var entityUpdatedEventTriggered = false;

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigations>>(data =>
        {
            entityUpdatedEventTriggered = true;

            // The Aggregate will not be updated
            data.Entity.ConcurrencyStamp.ShouldBe(latestConcurrencyStamp);
            data.Entity.LastModificationTime.ShouldBe(lastModificationTime);
            return Task.CompletedTask;
        });

        // Test with value object
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.AppEntityWithValueObjectAddress = new AppEntityWithValueObjectAddress("Turkey");
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();

        // Test with one to one
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.OneToOne = new AppEntityWithNavigationChildOneToOne
            {
                ChildName = "ChildName"
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();

        // Test with one to many
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.OneToMany = new List<AppEntityWithNavigationChildOneToMany>()
            {
                new AppEntityWithNavigationChildOneToMany
                {
                    AppEntityWithNavigationId = entity.Id,
                    ChildName = "ChildName1"
                }
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();

        // Test with many to many
        entityUpdatedEventTriggered = false;
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.ManyToMany = new List<AppEntityWithNavigationChildManyToMany>()
            {
                new AppEntityWithNavigationChildManyToMany
                {
                    ChildName = "ChildName1"
                }
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        entityUpdatedEventTriggered.ShouldBeTrue();
    }
}

public class AbpEfCoreDomainEvents_UpdateAggregateRootWhenNavigationChanges_Tests :  AbpEfCoreDomainEvents_Disable_UpdateAggregateRoot_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityChangeOptions>(options =>
        {
            options.UpdateAggregateRootWhenNavigationChanges = false;
        });

        base.AfterAddApplication(services);
    }
}

public class AbpEfCoreDomainEvents_IgnoredUpdateAggregateRootSelectors_Test :  AbpEfCoreDomainEvents_Disable_UpdateAggregateRoot_Tests
{
    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityChangeOptions>(options =>
        {
            options.IgnoredUpdateAggregateRootSelectors.Add("AppEntityWithValueObjectAddress", x => x == typeof(AppEntityWithNavigations));
        });

        base.AfterAddApplication(services);
    }
}
