using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.Repositories;

public class ReadOnlyRepository_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
{
    [Fact]
    public async Task ReadOnlyRepository_Should_NoTracking()
    {
        // Non-read-only repository tracking default
        await WithUnitOfWorkAsync(async () =>
        {
            var repository = GetRequiredService<IRepository<Person, Guid>>();
            var db = await repository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);
            var list = await repository.GetListAsync();
            list.Count.ShouldBeGreaterThan(0);
            db.ChangeTracker.Entries().Count().ShouldBe(list.Count);
        });

        // Read-only repository no tracking default
        await WithUnitOfWorkAsync(async () =>
        {
            var readonlyRepository = GetRequiredService<IReadOnlyRepository<Person, Guid>>();
            var db = await readonlyRepository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);
            var list = await readonlyRepository.GetListAsync();
            list.Count.ShouldBeGreaterThan(0);
            db.ChangeTracker.Entries().Count().ShouldBe(0);
        });

        // Read-only repository can tracking manually by AsTracking
        await WithUnitOfWorkAsync(async () =>
        {
            var readonlyRepository = GetRequiredService<IReadOnlyRepository<Person, Guid>>();
            var db = await readonlyRepository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);
            var list = await (await readonlyRepository.ToEfCoreRepository().GetQueryableAsync()).AsTracking().ToListAsync();
            list.Count.ShouldBeGreaterThan(0);
            db.ChangeTracker.Entries().Count().ShouldBe(list.Count);
        });
    }

    [Fact]
    public async Task ReadOnlyRepository_Should_Throw_AbpRepositoryIsReadOnlyException_When_Write_Method_Call()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var repository = GetRequiredService<IRepository<Person, Guid>>();
            await repository.ToEfCoreRepository().InsertAsync(new Person(Guid.NewGuid(), "test", 18));
            var person = await repository.ToEfCoreRepository().FirstOrDefaultAsync();
            person.ShouldNotBeNull();
        });

        await WithUnitOfWorkAsync(async () =>
        {
            await Assert.ThrowsAsync<AbpRepositoryIsReadOnlyException>(async () =>
            {
                var readonlyRepository = GetRequiredService<IReadOnlyRepository<Person, Guid>>();
                await readonlyRepository.ToEfCoreRepository().As<EfCoreRepository<TestAppDbContext, Person, Guid>>().InsertAsync(new Person(Guid.NewGuid(), "test readonly", 18));
            });
        });
    }
}
