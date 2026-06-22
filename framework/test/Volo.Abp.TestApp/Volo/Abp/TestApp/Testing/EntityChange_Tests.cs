using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
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

    [Fact]
    public async Task Should_Detect_Navigation_Changes_On_Second_SaveChanges_After_Remove_And_Add_OneToMany()
    {
        var entityId = Guid.NewGuid();
        var child1Id = Guid.NewGuid();

        await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity")
        {
            OneToMany = new List<AppEntityWithNavigationChildOneToMany>()
            {
                new AppEntityWithNavigationChildOneToMany(child1Id)
                {
                    AppEntityWithNavigationId = entityId,
                    ChildName = "Child1"
                }
            }
        });

        var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        string concurrencyStampAfterFirstSave = null;

        // Within a single UoW, remove Child1 (first SaveChanges), then add Child2 (second SaveChanges).
        // Before the fix, the second SaveChanges would not detect the navigation change
        // because the entity entries were removed after the first SaveChanges.
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            var originalConcurrencyStamp = entity.ConcurrencyStamp;

            // Remove Child1
            entity.OneToMany.Clear();
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
            await unitOfWorkManager.Current!.SaveChangesAsync();

            // ConcurrencyStamp should have been updated after the first navigation change
            entity.ConcurrencyStamp.ShouldNotBe(originalConcurrencyStamp);
            concurrencyStampAfterFirstSave = entity.ConcurrencyStamp;

            // Add Child2
            entity.OneToMany.Add(new AppEntityWithNavigationChildOneToMany(Guid.NewGuid())
            {
                AppEntityWithNavigationId = entityId,
                ChildName = "Child2"
            });
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });

        // After UoW completes, verify ConcurrencyStamp was updated again by the second SaveChanges
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.ConcurrencyStamp.ShouldNotBe(concurrencyStampAfterFirstSave);
            entity.OneToMany.Count.ShouldBe(1);
            entity.OneToMany[0].ChildName.ShouldBe("Child2");
        });
    }

    [Fact]
    public async Task Should_Detect_Navigation_Changes_On_Second_SaveChanges_After_Remove_And_Add_ManyToMany()
    {
        var entityId = Guid.NewGuid();

        await AppEntityWithNavigationsRepository.InsertAsync(new AppEntityWithNavigations(entityId, "TestEntity")
        {
            ManyToMany = new List<AppEntityWithNavigationChildManyToMany>()
            {
                new AppEntityWithNavigationChildManyToMany
                {
                    ChildName = "ManyToManyChild1"
                }
            }
        });

        var unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

        string concurrencyStampAfterFirstSave = null;

        // Within a single UoW, remove ManyToManyChild1 (first SaveChanges), then add ManyToManyChild2 (second SaveChanges).
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            var originalConcurrencyStamp = entity.ConcurrencyStamp;

            // Remove ManyToManyChild1
            entity.ManyToMany.Clear();
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
            await unitOfWorkManager.Current!.SaveChangesAsync();

            // ConcurrencyStamp should have been updated after the first navigation change
            entity.ConcurrencyStamp.ShouldNotBe(originalConcurrencyStamp);
            concurrencyStampAfterFirstSave = entity.ConcurrencyStamp;

            // Add ManyToManyChild2
            entity.ManyToMany.Add(new AppEntityWithNavigationChildManyToMany
            {
                ChildName = "ManyToManyChild2"
            });
            await AppEntityWithNavigationsRepository.UpdateAsync(entity);
        });

        // After UoW completes, verify ConcurrencyStamp was updated again by the second SaveChanges
        await WithUnitOfWorkAsync(async () =>
        {
            var entity = await AppEntityWithNavigationsRepository.GetAsync(entityId);
            entity.ConcurrencyStamp.ShouldNotBe(concurrencyStampAfterFirstSave);
            entity.ManyToMany.Count.ShouldBe(1);
            entity.ManyToMany[0].ChildName.ShouldBe("ManyToManyChild2");
        });
    }
}
