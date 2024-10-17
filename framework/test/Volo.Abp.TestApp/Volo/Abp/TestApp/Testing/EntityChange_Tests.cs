using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class EntityChange_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly IRepository<AppEntityWithNavigations, Guid> AppEntityWithNavigationsRepository;

    protected EntityChange_Tests()
    {
        AppEntityWithNavigationsRepository = GetRequiredService<IRepository<AppEntityWithNavigations, Guid>>();
    }

    [Fact]
    public async Task Should_Update_AbpConcepts_Properties_When_Entity_Or_Its_Navigation_Property_Changed()
    {
        var entityId = Guid.NewGuid();
        var entity = await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity"));
        var concurrencyStamp = entity.ConcurrencyStamp;
        var lastModificationTime = entity.LastModificationTime;

        // Test with simple property
        await WithUnitOfWorkAsync(async () =>
        {
            entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.Name = Guid.NewGuid().ToString();
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        concurrencyStamp.ShouldNotBe(entity.ConcurrencyStamp);
        lastModificationTime.ShouldNotBe(entity.LastModificationTime);
        concurrencyStamp = entity.ConcurrencyStamp;
        lastModificationTime = entity.LastModificationTime;

        // Test with value object
        await WithUnitOfWorkAsync(async () =>
        {
            entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.AppEntityWithValueObjectAddress = new AppEntityWithValueObjectAddress("Turkey");
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        concurrencyStamp.ShouldNotBe(entity.ConcurrencyStamp);
        lastModificationTime.ShouldNotBe(entity.LastModificationTime);
        concurrencyStamp = entity.ConcurrencyStamp;
        lastModificationTime = entity.LastModificationTime;

        // Test with one to one
        await WithUnitOfWorkAsync(async () =>
        {
            entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.OneToOne = new AppEntityWithNavigationChildOneToOne
            {
                ChildName = "ChildName"
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        concurrencyStamp.ShouldNotBe(entity.ConcurrencyStamp);
        lastModificationTime.ShouldNotBe(entity.LastModificationTime);
        concurrencyStamp = entity.ConcurrencyStamp;
        lastModificationTime = entity.LastModificationTime;

        // Test with one to many
        await WithUnitOfWorkAsync(async () =>
        {
            entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
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
        concurrencyStamp.ShouldNotBe(entity.ConcurrencyStamp);
        lastModificationTime.ShouldNotBe(entity.LastModificationTime);
        concurrencyStamp = entity.ConcurrencyStamp;
        lastModificationTime = entity.LastModificationTime;

        // Test with many to many
        await WithUnitOfWorkAsync(async () =>
        {
            entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.ManyToMany = new List<AppEntityWithNavigationChildManyToMany>()
            {
                new AppEntityWithNavigationChildManyToMany
                {
                    ChildName = "ChildName1"
                }
            };
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });
        concurrencyStamp.ShouldNotBe(entity.ConcurrencyStamp);
        lastModificationTime.ShouldNotBe(entity.LastModificationTime);

    }
}
