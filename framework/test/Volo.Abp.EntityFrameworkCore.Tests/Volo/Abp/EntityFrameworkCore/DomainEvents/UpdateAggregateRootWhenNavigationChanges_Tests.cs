using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DomainEvents;

public class UpdateAggregateRootWhenNavigationChanges_Tests : EntityFrameworkCoreTestBase
{
    private readonly IRepository<AppEntityWithForeignKeyOnly, Guid> _entityWithForeignKeyOnlyRepository;
    private readonly IRepository<AppEntityWithForeignKeyOnlyChild, Guid> _childRepository;
    private readonly IRepository<AppEntityWithForeignKeyOnlyOwner, Guid> _ownerRepository;
    private readonly IRepository<AppEntityWithForeignKeyOnlyEntityChild, Guid> _entityChildRepository;
    private readonly IRepository<AppEntityWithNavigations, Guid> _entityWithNavigationsRepository;
    private readonly IRepository<AppEntityWithNavigationsForeign, Guid> _entityWithNavigationsForeignRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly ILocalEventBus _localEventBus;

    public UpdateAggregateRootWhenNavigationChanges_Tests()
    {
        _entityWithForeignKeyOnlyRepository = GetRequiredService<IRepository<AppEntityWithForeignKeyOnly, Guid>>();
        _childRepository = GetRequiredService<IRepository<AppEntityWithForeignKeyOnlyChild, Guid>>();
        _ownerRepository = GetRequiredService<IRepository<AppEntityWithForeignKeyOnlyOwner, Guid>>();
        _entityChildRepository = GetRequiredService<IRepository<AppEntityWithForeignKeyOnlyEntityChild, Guid>>();
        _entityWithNavigationsRepository = GetRequiredService<IRepository<AppEntityWithNavigations, Guid>>();
        _entityWithNavigationsForeignRepository = GetRequiredService<IRepository<AppEntityWithNavigationsForeign, Guid>>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _localEventBus = GetRequiredService<ILocalEventBus>();
    }

    [Fact]
    public async Task Should_Not_Update_Principal_Entity_Without_Navigation_Property()
    {
        var principalId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithForeignKeyOnlyRepository.InsertAsync(
                new AppEntityWithForeignKeyOnly(principalId, "Principal"));
        });

        var concurrencyStamp = (await _entityWithForeignKeyOnlyRepository.GetAsync(principalId)).ConcurrencyStamp;

        var principalUpdatedEventTriggered = false;
        _localEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithForeignKeyOnly>>(_ =>
        {
            principalUpdatedEventTriggered = true;
            return Task.CompletedTask;
        });

        await WithUnitOfWorkAsync(async () =>
        {
            // The principal has to be tracked to be a candidate for the aggregate root update.
            await _entityWithForeignKeyOnlyRepository.GetAsync(principalId);

            await _childRepository.InsertAsync(
                new AppEntityWithForeignKeyOnlyChild(Guid.NewGuid(), principalId, "Child"));
        });

        principalUpdatedEventTriggered.ShouldBeFalse();
        (await _entityWithForeignKeyOnlyRepository.GetAsync(principalId)).ConcurrencyStamp.ShouldBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Not_Update_Principal_Entity_Without_Navigation_Property_On_Update_And_Delete()
    {
        var principalId = Guid.NewGuid();
        var childId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithForeignKeyOnlyRepository.InsertAsync(
                new AppEntityWithForeignKeyOnly(principalId, "Principal"));
            await _childRepository.InsertAsync(
                new AppEntityWithForeignKeyOnlyChild(childId, principalId, "Child"));
        });

        var concurrencyStamp = (await _entityWithForeignKeyOnlyRepository.GetAsync(principalId)).ConcurrencyStamp;

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithForeignKeyOnlyRepository.GetAsync(principalId);

            var child = await _childRepository.GetAsync(childId);
            child.Name = "Child-Updated";
            await _childRepository.UpdateAsync(child);
        });

        (await _entityWithForeignKeyOnlyRepository.GetAsync(principalId)).ConcurrencyStamp.ShouldBe(concurrencyStamp);

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithForeignKeyOnlyRepository.GetAsync(principalId);

            await _childRepository.DeleteAsync(childId);
        });

        (await _entityWithForeignKeyOnlyRepository.GetAsync(principalId)).ConcurrencyStamp.ShouldBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Update_The_Owner_But_Not_The_Referenced_Aggregate_Root_Of_A_Child_Entity()
    {
        var ownerId = Guid.NewGuid();
        var referencedId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _ownerRepository.InsertAsync(new AppEntityWithForeignKeyOnlyOwner(ownerId, "Owner"));
            await _entityWithForeignKeyOnlyRepository.InsertAsync(
                new AppEntityWithForeignKeyOnly(referencedId, "Referenced"));
        });

        var ownerStamp = (await _ownerRepository.GetAsync(ownerId)).ConcurrencyStamp;
        var referencedStamp = (await _entityWithForeignKeyOnlyRepository.GetAsync(referencedId)).ConcurrencyStamp;

        await WithUnitOfWorkAsync(async () =>
        {
            await _ownerRepository.GetAsync(ownerId);
            await _entityWithForeignKeyOnlyRepository.GetAsync(referencedId);

            await _entityChildRepository.InsertAsync(
                new AppEntityWithForeignKeyOnlyEntityChild(Guid.NewGuid(), ownerId, referencedId, "Child"));
        });

        (await _ownerRepository.GetAsync(ownerId)).ConcurrencyStamp.ShouldNotBe(ownerStamp);
        (await _entityWithForeignKeyOnlyRepository.GetAsync(referencedId)).ConcurrencyStamp.ShouldBe(referencedStamp);
    }

    [Fact]
    public async Task Should_Update_Aggregate_Root_When_Owned_Entity_Changes()
    {
        var entityId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(entityId, "Entity"));
        });

        var concurrencyStamp = (await _entityWithNavigationsRepository.GetAsync(entityId)).ConcurrencyStamp;

        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await _entityWithNavigationsRepository.GetAsync(entityId);
            entity.AppEntityWithValueObjectAddress = new AppEntityWithValueObjectAddress("Turkey");
            await _entityWithNavigationsRepository.UpdateAsync(entity);
        });

        (await _entityWithNavigationsRepository.GetAsync(entityId)).ConcurrencyStamp.ShouldNotBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Update_Aggregate_Root_When_Navigation_Changes_By_Default()
    {
        var entityId = Guid.NewGuid();
        var foreignId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsForeignRepository.InsertAsync(
                new AppEntityWithNavigationsForeign(foreignId, "Foreign"));
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(entityId, "Entity"));
        });

        var concurrencyStamp = (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp;

        var foreignUpdatedEventTriggered = false;
        _localEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationsForeign>>(_ =>
        {
            foreignUpdatedEventTriggered = true;
            return Task.CompletedTask;
        });

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsForeignRepository.GetAsync(foreignId);

            var entity = await _entityWithNavigationsRepository.GetAsync(entityId);
            entity.AppEntityWithNavigationForeignId = foreignId;
            await _entityWithNavigationsRepository.UpdateAsync(entity);
        });

        foreignUpdatedEventTriggered.ShouldBeTrue();
        (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp.ShouldNotBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Not_Update_Aggregate_Root_But_Still_Publish_Event_When_Disabled_For_The_Unit_Of_Work()
    {
        var entityId = Guid.NewGuid();
        var foreignId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsForeignRepository.InsertAsync(
                new AppEntityWithNavigationsForeign(foreignId, "Foreign"));
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(entityId, "Entity"));
        });

        var concurrencyStamp = (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp;

        var foreignUpdatedEventTriggered = false;
        _localEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigationsForeign>>(_ =>
        {
            foreignUpdatedEventTriggered = true;
            return Task.CompletedTask;
        });

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            using (uow.DisableUpdateAggregateRootWhenNavigationChanges())
            {
                await _entityWithNavigationsForeignRepository.GetAsync(foreignId);

                var entity = await _entityWithNavigationsRepository.GetAsync(entityId);
                entity.AppEntityWithNavigationForeignId = foreignId;
                await _entityWithNavigationsRepository.UpdateAsync(entity);

                await uow.CompleteAsync();
            }
        }

        foreignUpdatedEventTriggered.ShouldBeTrue();
        (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp.ShouldBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Update_Aggregate_Root_Of_Its_Own_Changes_While_Disabled()
    {
        var entityId = Guid.NewGuid();
        var foreignId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsForeignRepository.InsertAsync(
                new AppEntityWithNavigationsForeign(foreignId, "Foreign"));
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(entityId, "Entity"));
        });

        var concurrencyStamp = (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp;

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            using (uow.DisableUpdateAggregateRootWhenNavigationChanges())
            {
                // The foreign entity is changed by itself, not by a navigation change.
                var foreign = await _entityWithNavigationsForeignRepository.GetAsync(foreignId);
                foreign.Name = "Foreign-Updated";
                await _entityWithNavigationsForeignRepository.UpdateAsync(foreign);

                await uow.CompleteAsync();
            }
        }

        var updatedForeign = await _entityWithNavigationsForeignRepository.GetAsync(foreignId);
        updatedForeign.Name.ShouldBe("Foreign-Updated");
        updatedForeign.ConcurrencyStamp.ShouldNotBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Use_The_Current_Value_On_Each_Save_Changes_Of_The_Same_Unit_Of_Work()
    {
        var firstEntityId = Guid.NewGuid();
        var secondEntityId = Guid.NewGuid();
        var foreignId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsForeignRepository.InsertAsync(
                new AppEntityWithNavigationsForeign(foreignId, "Foreign"));
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(firstEntityId, "Entity1"));
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(secondEntityId, "Entity2"));
        });

        var concurrencyStamp = (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp;

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            using (uow.DisableUpdateAggregateRootWhenNavigationChanges())
            {
                await _entityWithNavigationsForeignRepository.GetAsync(foreignId);

                var firstEntity = await _entityWithNavigationsRepository.GetAsync(firstEntityId);
                firstEntity.AppEntityWithNavigationForeignId = foreignId;
                await _entityWithNavigationsRepository.UpdateAsync(firstEntity, autoSave: true);
            }

            (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp.ShouldBe(concurrencyStamp);

            // The setting is restored, the second save has to update the aggregate root.
            var secondEntity = await _entityWithNavigationsRepository.GetAsync(secondEntityId);
            secondEntity.AppEntityWithNavigationForeignId = foreignId;
            await _entityWithNavigationsRepository.UpdateAsync(secondEntity, autoSave: true);

            await uow.CompleteAsync();
        }

        (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp.ShouldNotBe(concurrencyStamp);
    }

    [Fact]
    public async Task Should_Restore_The_Previous_Value_On_Dispose()
    {
        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            uow.GetUpdateAggregateRootWhenNavigationChangesOrNull().ShouldBeNull();

            using (uow.DisableUpdateAggregateRootWhenNavigationChanges())
            {
                uow.GetUpdateAggregateRootWhenNavigationChangesOrNull().ShouldBe(false);

                using (uow.EnableUpdateAggregateRootWhenNavigationChanges())
                {
                    uow.GetUpdateAggregateRootWhenNavigationChangesOrNull().ShouldBe(true);
                }

                uow.GetUpdateAggregateRootWhenNavigationChangesOrNull().ShouldBe(false);
            }

            uow.GetUpdateAggregateRootWhenNavigationChangesOrNull().ShouldBeNull();

            await uow.CompleteAsync();
        }
    }
}

public class UpdateAggregateRootWhenNavigationChanges_Globally_Disabled_Tests : EntityFrameworkCoreTestBase
{
    private readonly IRepository<AppEntityWithNavigations, Guid> _entityWithNavigationsRepository;
    private readonly IRepository<AppEntityWithNavigationsForeign, Guid> _entityWithNavigationsForeignRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UpdateAggregateRootWhenNavigationChanges_Globally_Disabled_Tests()
    {
        _entityWithNavigationsRepository = GetRequiredService<IRepository<AppEntityWithNavigations, Guid>>();
        _entityWithNavigationsForeignRepository = GetRequiredService<IRepository<AppEntityWithNavigationsForeign, Guid>>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityChangeOptions>(options =>
        {
            options.UpdateAggregateRootWhenNavigationChanges = false;
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task Should_Update_Aggregate_Root_When_Enabled_For_The_Unit_Of_Work()
    {
        var entityId = Guid.NewGuid();
        var foreignId = Guid.NewGuid();

        await WithUnitOfWorkAsync(async () =>
        {
            await _entityWithNavigationsForeignRepository.InsertAsync(
                new AppEntityWithNavigationsForeign(foreignId, "Foreign"));
            await _entityWithNavigationsRepository.InsertAsync(
                new AppEntityWithNavigations(entityId, "Entity"));
        });

        var concurrencyStamp = (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp;

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            using (uow.EnableUpdateAggregateRootWhenNavigationChanges())
            {
                await _entityWithNavigationsForeignRepository.GetAsync(foreignId);

                var entity = await _entityWithNavigationsRepository.GetAsync(entityId);
                entity.AppEntityWithNavigationForeignId = foreignId;
                await _entityWithNavigationsRepository.UpdateAsync(entity);

                await uow.CompleteAsync();
            }
        }

        (await _entityWithNavigationsForeignRepository.GetAsync(foreignId)).ConcurrencyStamp.ShouldNotBe(concurrencyStamp);
    }
}
