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

public class AbpEntityChangeOptions_DomainEvents_Tests : AbpEntityChangeOptions_DomainEvents_Tests<AbpEntityFrameworkCoreTestModule>
{
}

public class AbpEfCoreDomainEvents_Tests : EntityFrameworkCoreTestBase
{
    protected readonly IRepository<AppEntityWithNavigations, Guid> AppEntityWithNavigationsRepository;
    protected readonly ILocalEventBus LocalEventBus;

    public AbpEfCoreDomainEvents_Tests()
    {
        AppEntityWithNavigationsRepository = GetRequiredService<IRepository<AppEntityWithNavigations, Guid>>();
        LocalEventBus = GetRequiredService<ILocalEventBus>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpEntityOptions>(options =>
        {
            options.Entity<AppEntityWithNavigations>(opt =>
            {
                opt.DefaultWithDetailsFunc = q => q;
            });
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public async Task Should_Trigger_Domain_Events_For_Aggregate_Root_When_EnsureCollectionLoaded_Navigation_Changes_Tests()
    {
        var entityId = Guid.NewGuid();

        await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity")
        {
            OneToMany = new List<AppEntityWithNavigationChildOneToMany>()
            {
                new AppEntityWithNavigationChildOneToMany
                {
                    ChildName = "ChildName1"
                }
            }
        });

        var entityUpdatedEventTriggered = false;

        LocalEventBus.Subscribe<EntityUpdatedEventData<AppEntityWithNavigations>>(data =>
        {
            entityUpdatedEventTriggered = !entityUpdatedEventTriggered;
            return Task.CompletedTask;
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
