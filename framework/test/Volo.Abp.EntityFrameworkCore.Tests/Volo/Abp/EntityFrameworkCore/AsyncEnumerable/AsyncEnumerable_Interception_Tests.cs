using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.ChangeTracking;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.AsyncEnumerable;

public interface IStreamingPeopleAppService : IApplicationService
{
    IAsyncEnumerable<string> GetNamesAsync();

    IAsyncEnumerable<string> IncreaseAgesAsync(int failAfter = -1);

    IAsyncEnumerable<Guid?> GetUnitOfWorkIdsAsync();

    IAsyncEnumerable<bool> GetIsTransactionalAsync();

    IAsyncEnumerable<bool> StreamIsTransactionalAsync();

    IAsyncEnumerable<string> GetNamesWithWorkBeforeReturningAsync();

    IAsyncEnumerable<int> GetItemsWithOutParameter(out bool hadUnitOfWork);

    IAsyncEnumerable<string> IncreaseAgeThenFailOnCurrent();

    IAsyncEnumerable<bool?> GetChangeTrackingStatesAsync();
}

public class StreamingTracker : ISingletonDependency
{
    public bool SourceDisposed { get; set; }

    public bool UnitOfWorkCompleted { get; set; }

    public bool CallUnitOfWorkCompleted { get; set; }
}

public class StreamingPeopleAppService : ApplicationService, IStreamingPeopleAppService
{
    private readonly IRepository<Person, Guid> _personRepository;
    private readonly StreamingTracker _tracker;

    public StreamingPeopleAppService(IRepository<Person, Guid> personRepository, StreamingTracker tracker)
    {
        _personRepository = personRepository;
        _tracker = tracker;
    }

    public virtual async IAsyncEnumerable<string> GetNamesAsync()
    {
        var queryable = await _personRepository.GetQueryableAsync();
        await foreach (var person in queryable.OrderBy(p => p.Name).AsAsyncEnumerable())
        {
            yield return person.Name;
        }
    }

    public virtual async IAsyncEnumerable<string> IncreaseAgesAsync(int failAfter = -1)
    {
        CurrentUnitOfWork!.OnCompleted(() =>
        {
            _tracker.UnitOfWorkCompleted = true;
            return Task.CompletedTask;
        });

        try
        {
            var people = await _personRepository.GetListAsync();
            var count = 0;
            foreach (var person in people.OrderBy(p => p.Name))
            {
                if (++count == failAfter)
                {
                    throw new InvalidOperationException("Failed while streaming");
                }

                person.Age += 100;
                await _personRepository.UpdateAsync(person, autoSave: true);

                yield return person.Name;
            }
        }
        finally
        {
            _tracker.SourceDisposed = true;
        }
    }

    public virtual async IAsyncEnumerable<Guid?> GetUnitOfWorkIdsAsync()
    {
        await Task.Yield();
        yield return CurrentUnitOfWork?.Id;
    }

    public virtual async IAsyncEnumerable<bool> GetIsTransactionalAsync()
    {
        await Task.Yield();
        yield return CurrentUnitOfWork!.Options.IsTransactional;
    }

    public virtual async IAsyncEnumerable<bool> StreamIsTransactionalAsync()
    {
        await Task.Yield();
        yield return CurrentUnitOfWork!.Options.IsTransactional;
    }

    public virtual IAsyncEnumerable<string> GetNamesWithWorkBeforeReturningAsync()
    {
        CurrentUnitOfWork!.OnCompleted(() =>
        {
            _tracker.CallUnitOfWorkCompleted = true;
            return Task.CompletedTask;
        });

        return GetNamesCoreAsync();
    }

    public virtual IAsyncEnumerable<int> GetItemsWithOutParameter(out bool hadUnitOfWork)
    {
        hadUnitOfWork = CurrentUnitOfWork != null;
        return GetOneAsync();
    }

    [DisableEntityChangeTracking]
    public virtual async IAsyncEnumerable<bool?> GetChangeTrackingStatesAsync()
    {
        var changeTrackingProvider = LazyServiceProvider.LazyGetRequiredService<IEntityChangeTrackingProvider>();
        await Task.Delay(1).ConfigureAwait(false);
        yield return changeTrackingProvider.Enabled;
        await Task.Delay(1).ConfigureAwait(false);
        yield return changeTrackingProvider.Enabled;
    }

    public virtual IAsyncEnumerable<string> IncreaseAgeThenFailOnCurrent()
    {
        return new FailingCurrentEnumerable(_personRepository);
    }

    private async IAsyncEnumerable<string> GetNamesCoreAsync()
    {
        var queryable = await _personRepository.GetQueryableAsync();
        await foreach (var person in queryable.OrderBy(p => p.Name).AsAsyncEnumerable())
        {
            yield return person.Name;
        }
    }

    private static async IAsyncEnumerable<int> GetOneAsync()
    {
        await Task.Yield();
        yield return 1;
    }

    private class FailingCurrentEnumerable : IAsyncEnumerable<string>, IAsyncEnumerator<string>
    {
        private readonly IRepository<Person, Guid> _personRepository;

        public FailingCurrentEnumerable(IRepository<Person, Guid> personRepository)
        {
            _personRepository = personRepository;
        }

        public string Current => throw new InvalidOperationException("Failed to read the current item");

        public IAsyncEnumerator<string> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default) => this;

        public async ValueTask<bool> MoveNextAsync()
        {
            foreach (var person in await _personRepository.GetListAsync())
            {
                person.Age += 100;
                await _personRepository.UpdateAsync(person, autoSave: true);
            }

            return true;
        }

        public ValueTask DisposeAsync() => default;
    }
}

public class AsyncEnumerable_Interception_Tests : EntityFrameworkCoreTestBase
{
    private readonly IStreamingPeopleAppService _appService;
    private readonly IRepository<Person, Guid> _personRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly StreamingTracker _tracker;

    public AsyncEnumerable_Interception_Tests()
    {
        _appService = GetRequiredService<IStreamingPeopleAppService>();
        _personRepository = GetRequiredService<IRepository<Person, Guid>>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _tracker = GetRequiredService<StreamingTracker>();
    }

    [Fact]
    public async Task Should_Have_Unit_Of_Work_While_Enumerating()
    {
        _unitOfWorkManager.Current.ShouldBeNull();

        var ids = await _appService.GetUnitOfWorkIdsAsync().ToStreamListAsync();

        ids.Single().ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Decide_Transaction_Like_Task_Returning_Methods()
    {
        (await _appService.GetIsTransactionalAsync().ToStreamListAsync()).ShouldBe(new[] { false });
        (await _appService.StreamIsTransactionalAsync().ToStreamListAsync()).ShouldBe(new[] { true });
    }

    [Fact]
    public async Task Should_Query_Database_While_Enumerating()
    {
        var names = await _appService.GetNamesAsync().ToStreamListAsync();

        names.ShouldContain("Douglas");
    }

    [Fact]
    public async Task Should_Save_Changes_After_Enumeration_Completed()
    {
        await InsertPersonAsync("Aaron");
        var originalAges = await GetAgesAsync();

        var names = await _appService.IncreaseAgesAsync().ToStreamListAsync();

        names.Count.ShouldBe(originalAges.Count);
        _tracker.UnitOfWorkCompleted.ShouldBeTrue();
        var newAges = await GetAgesAsync();
        foreach (var (id, age) in originalAges)
        {
            newAges[id].ShouldBe(age + 100);
        }
    }

    [Fact]
    public async Task Should_Rollback_When_Enumeration_Throws_After_Items_Were_Delivered()
    {
        await InsertPersonAsync("Aaron");
        var originalAges = await GetAgesAsync();
        var names = new List<string>();

        var exception = await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var name in _appService.IncreaseAgesAsync(failAfter: 2))
            {
                names.Add(name);
            }
        });

        exception.Message.ShouldBe("Failed while streaming");
        names.ShouldBe(new[] { "Aaron" });
        _tracker.SourceDisposed.ShouldBeTrue();
        _tracker.UnitOfWorkCompleted.ShouldBeFalse();
        (await GetAgesAsync()).ShouldBe(originalAges);
    }

    [Fact]
    public async Task Should_Complete_Unit_Of_Work_When_Consumer_Stops_Early()
    {
        var aaronId = await InsertPersonAsync("Aaron");
        var originalAges = await GetAgesAsync();

        await foreach (var name in _appService.IncreaseAgesAsync())
        {
            name.ShouldBe("Aaron");
            break;
        }

        _tracker.SourceDisposed.ShouldBeTrue();
        _tracker.UnitOfWorkCompleted.ShouldBeTrue();
        var newAges = await GetAgesAsync();
        foreach (var (id, age) in originalAges)
        {
            newAges[id].ShouldBe(id == aaronId ? age + 100 : age);
        }
    }

    [Fact]
    public async Task Should_Join_Outer_Unit_Of_Work()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            var outerId = _unitOfWorkManager.Current!.Id;

            await foreach (var id in _appService.GetUnitOfWorkIdsAsync())
            {
                id.ShouldBe(outerId);
            }

            await foreach (var _ in _appService.GetNamesAsync())
            {
                // The consumer uses the same DbContext as the stream between two items.
                (await _personRepository.GetCountAsync()).ShouldBeGreaterThan(0);
            }
        });
    }

    [Fact]
    public async Task Should_Leave_Completing_The_Reserved_Unit_Of_Work_To_Its_Owner()
    {
        await InsertPersonAsync("Aaron");
        var originalAges = await GetAgesAsync();

        using (var reservedUnitOfWork = _unitOfWorkManager.Reserve(UnitOfWork.UnitOfWorkReservationName))
        {
            (await _appService.IncreaseAgesAsync().ToStreamListAsync()).Count.ShouldBe(originalAges.Count);

            reservedUnitOfWork.IsCompleted.ShouldBeFalse();
            _tracker.UnitOfWorkCompleted.ShouldBeFalse();

            await reservedUnitOfWork.CompleteAsync();
        }

        _tracker.UnitOfWorkCompleted.ShouldBeTrue();
        var newAges = await GetAgesAsync();
        foreach (var (id, age) in originalAges)
        {
            newAges[id].ShouldBe(age + 100);
        }
    }

    [Fact]
    public async Task Should_Apply_Change_Tracking_Attribute_While_Enumerating()
    {
        (await _appService.GetChangeTrackingStatesAsync().ToStreamListAsync()).ShouldBe(new bool?[] { false, false });
    }

    [Fact]
    public async Task Should_Have_Unit_Of_Work_When_Creating_The_Sequence()
    {
        var names = await _appService.GetNamesWithWorkBeforeReturningAsync().ToStreamListAsync();

        names.ShouldContain("Douglas");
        _tracker.CallUnitOfWorkCompleted.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Have_Unit_Of_Work_When_Calling_Method_With_Out_Parameters()
    {
        var sequence = _appService.GetItemsWithOutParameter(out var hadUnitOfWork);

        hadUnitOfWork.ShouldBeTrue();
        (await sequence.ToStreamListAsync()).ShouldBe(new[] { 1 });
    }

    [Fact]
    public async Task Should_Rollback_When_Reading_The_Current_Item_Throws()
    {
        var originalAges = await GetAgesAsync();

        await Should.ThrowAsync<InvalidOperationException>(_appService.IncreaseAgeThenFailOnCurrent().ToStreamListAsync());

        (await GetAgesAsync()).ShouldBe(originalAges);
    }

    private async Task<Guid> InsertPersonAsync(string name)
    {
        var id = Guid.NewGuid();
        await WithUnitOfWorkAsync(() => _personRepository.InsertAsync(new Person(id, name, 20)));
        return id;
    }

    private async Task<Dictionary<Guid, int>> GetAgesAsync()
    {
        return await WithUnitOfWorkAsync(async () =>
            (await _personRepository.GetListAsync(includeDetails: false)).ToDictionary(p => p.Id, p => p.Age));
    }
}

internal static class AsyncEnumerableTestExtensions
{
    public static async Task<List<T>> ToStreamListAsync<T>(this IAsyncEnumerable<T> source)
    {
        var list = new List<T>();
        await foreach (var item in source)
        {
            list.Add(item);
        }

        return list;
    }
}
