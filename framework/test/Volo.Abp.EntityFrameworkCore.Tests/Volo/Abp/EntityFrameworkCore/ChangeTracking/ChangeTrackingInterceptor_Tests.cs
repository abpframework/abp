using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.ChangeTracking;

public class ChangeTrackingInterceptor_Tests : TestAppTestBase<AbpEntityFrameworkCoreTestModule>
{
    [Fact]
    public async Task ReadOnly_Repository_Should_Not_Track_Entities()
    {
        await AddSomePeopleAsync();

        var readOnlyRepository = GetRequiredService<IReadOnlyRepository<Person, Guid>>();

        await WithUnitOfWorkAsync(async () =>
        {
            var db = await readOnlyRepository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);

            var service = GetRequiredService<MyReadOnlyService>();
            var list = await service.GetPeoplesAsync();
            list.Count.ShouldBeGreaterThan(0);

            // RepositoryInterceptor always not track entities
            db.ChangeTracker.Entries().Count().ShouldBe(0);
        });
    }

    [Fact]
    public async Task RepositoryInterceptor_Test()
    {
        await AddSomePeopleAsync();

        var repository = GetRequiredService<IRepository<Person, Guid>>();

        await WithUnitOfWorkAsync(async () =>
        {
            var service = GetRequiredService<MyService>();
            var db = await repository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);

            var list = await service.GetPeoplesAsync();
            list.Count.ShouldBeGreaterThan(0);

            db.ChangeTracker.Entries().Count().ShouldBe(1); // Track one entity from GetPeopleAsync
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var service = GetRequiredService<MyServiceEnableEntityChangeTracking>();
            var db = await repository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);

            var list = await service.GetPeoplesAsync();
            list.Count.ShouldBeGreaterThan(0);

            db.ChangeTracker.Entries().Count().ShouldBe(1); // Track one entity from GetPeoplesAsync
        });

        await WithUnitOfWorkAsync(async () =>
        {
            var service = GetRequiredService<MyServiceChangeTrackingByEntityChangeTrackingProvider>();
            var db = await repository.GetDbContextAsync();
            db.ChangeTracker.Entries().Count().ShouldBe(0);

            var entityChangeTrackingProvider = GetRequiredService<IEntityChangeTrackingProvider>();
            // Disable entity change tracking
            using (entityChangeTrackingProvider.Change(false))
            {
                var list = await service.GetPeoplesAsync();
                list.Count.ShouldBeGreaterThan(0);
                db.ChangeTracker.Entries().Count().ShouldBe(0);
            }
        });
    }

    private async Task AddSomePeopleAsync()
    {
        var repository = GetRequiredService<IRepository<Person, Guid>>();
        await repository.InsertAsync(new Person(Guid.NewGuid(), "people1", 18));
        await repository.InsertAsync(new Person(Guid.NewGuid(), "people2", 19));
        await repository.InsertAsync(new Person(Guid.NewGuid(), "people3", 20));
        await repository.InsertAsync(new Person(Guid.NewGuid(), "people4", 21));
    }
}

public class MyService : ITransientDependency
{
    private readonly IRepository<Person, Guid> _repository;

    public MyService(IRepository<Person, Guid> repository)
    {
        _repository = repository;
    }

    [DisableEntityChangeTracking]
    public virtual async Task<List<Person>> GetPeoplesAsync()
    {
        await GetPeopleAsync();
        return await _repository.GetListAsync();
    }

    [EnableEntityChangeTracking]
    public virtual async Task<Person> GetPeopleAsync()
    {
        var p1 = await _repository.FindAsync(x => x.Name == "people1");
        return p1;
    }
}

public class MyReadOnlyService : MyService
{
    public MyReadOnlyService(IReadOnlyRepository<Person, Guid> repository)
        : base(repository.As<IRepository<Person, Guid>>())
    {
    }
}


[EnableEntityChangeTracking]
public class MyServiceEnableEntityChangeTracking : ITransientDependency
{
    private readonly IRepository<Person, Guid> _repository;

    public MyServiceEnableEntityChangeTracking(IRepository<Person, Guid> repository)
    {
        _repository = repository;
    }

    public virtual async Task<List<Person>> GetPeoplesAsync()
    {
        var p1 = await GetPeopleAsync();
        var p2 = await _repository.FindAsync(x => x.Name == "people2");

        return new List<Person> {p1, p2};
    }

    [DisableEntityChangeTracking]
    public virtual async Task<Person> GetPeopleAsync()
    {
        var p1 = await _repository.FindAsync(x => x.Name == "people1");
        return p1;
    }
}

public class MyServiceChangeTrackingByEntityChangeTrackingProvider : ITransientDependency
{
    private readonly IRepository<Person, Guid> _repository;

    public MyServiceChangeTrackingByEntityChangeTrackingProvider(IRepository<Person, Guid> repository)
    {
        _repository = repository;
    }

    public virtual async Task<List<Person>> GetPeoplesAsync()
    {
        return await _repository.GetListAsync();
    }
}
