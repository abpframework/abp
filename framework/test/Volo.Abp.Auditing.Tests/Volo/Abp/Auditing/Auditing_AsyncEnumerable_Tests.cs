using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Auditing;

public class Auditing_AsyncEnumerable_Tests : AbpAuditingTestBase
{
    private readonly List<AuditLogInfo> _savedAuditLogs = new();
    private readonly List<(List<string> Comments, int[] ActionDurations)> _savedAuditLogSnapshots = new();
    private readonly IAuditingManager _auditingManager;
    private readonly MyStreamingAuditedObject _target;

    public Auditing_AsyncEnumerable_Tests()
    {
        _auditingManager = GetRequiredService<IAuditingManager>();
        _target = GetRequiredService<MyStreamingAuditedObject>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        var auditingStore = Substitute.For<IAuditingStore>();
        auditingStore.SaveAsync(Arg.Do<AuditLogInfo>(auditLog =>
        {
            _savedAuditLogs.Add(auditLog);
            _savedAuditLogSnapshots.Add((auditLog.Comments.ToList(), auditLog.Actions.Select(a => a.ExecutionDuration).ToArray()));
        }));
        services.Replace(ServiceDescriptor.Singleton(auditingStore));
    }

    [Fact]
    public async Task Should_Save_The_Call_And_The_Enumeration_As_Separate_Audit_Logs()
    {
        var sequence = _target.StreamItemsAsync(50);

        var callLog = _savedAuditLogs.ShouldHaveSingleItem();
        IsEnumeration(callLog.Actions.ShouldHaveSingleItem()).ShouldBeFalse();

        (await ToListAsync(sequence)).ShouldBe(new[] { 1, 2 });

        _savedAuditLogs.Count.ShouldBe(2);
        var enumerationAction = _savedAuditLogs[1].Actions.ShouldHaveSingleItem();
        IsEnumeration(enumerationAction).ShouldBeTrue();
        enumerationAction.MethodName.ShouldBe(nameof(MyStreamingAuditedObject.StreamItemsAsync));
        enumerationAction.ExecutionDuration.ShouldBeGreaterThanOrEqualTo(80);
        _savedAuditLogs[1].Exceptions.ShouldBeEmpty();
    }

    [Fact]
    public void Should_Not_Save_An_Enumeration_Audit_Log_If_The_Sequence_Is_Not_Enumerated()
    {
        _target.StreamItemsAsync(1);

        IsEnumeration(_savedAuditLogs.ShouldHaveSingleItem().Actions.ShouldHaveSingleItem()).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Save_The_Exception_Of_The_Sequence_In_The_Enumeration_Audit_Log()
    {
        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(_target.StreamItemsThenThrowAsync()));

        _savedAuditLogs.Count.ShouldBe(2);
        _savedAuditLogs[0].Exceptions.ShouldBeEmpty();
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
        _savedAuditLogs[1].Exceptions.ShouldContain(e => e.Message == "Stream failed");
    }

    [Fact]
    public async Task Should_Save_An_Audit_Log_For_Each_Enumeration()
    {
        var sequence = _target.StreamItemsAsync(1);

        await ToListAsync(sequence);
        await ToListAsync(sequence);

        _savedAuditLogs.Count.ShouldBe(3);
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
        IsEnumeration(_savedAuditLogs[2].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Copy_What_Was_Recorded_While_Creating_The_Sequence()
    {
        (await ToListAsync(_target.StreamItemsWithCommentAsync())).ShouldBe(new[] { 1, 2 });

        _savedAuditLogs.Count.ShouldBe(2);
        _savedAuditLogs[0].Comments.ShouldContain("Created the sequence");
        _savedAuditLogs[1].Comments.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Audit_The_Enumeration_When_The_Call_Recorded_An_Exception_Without_Throwing()
    {
        (await ToListAsync(_target.StreamItemsAfterHandledExceptionAsync())).ShouldBe(new[] { 1, 2 });

        _savedAuditLogs.Count.ShouldBe(2);
        _savedAuditLogs[0].Exceptions.ShouldContain(e => e.Message == "Handled while creating");
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
        _savedAuditLogs[1].Exceptions.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Record_What_Happens_During_Each_Enumeration_Only_In_Its_Own_Audit_Log()
    {
        var sequence = _target.StreamItemsWithEntityChangeAsync();

        await ToListAsync(sequence);
        await ToListAsync(sequence);

        _savedAuditLogs.Count.ShouldBe(3);
        _savedAuditLogs[0].EntityChanges.ShouldBeEmpty();
        var firstChange = _savedAuditLogs[1].EntityChanges.ShouldHaveSingleItem();
        var secondChange = _savedAuditLogs[2].EntityChanges.ShouldHaveSingleItem();
        firstChange.ShouldNotBeSameAs(secondChange);
    }

    [Fact]
    public async Task Should_Add_A_Nested_Sequence_To_The_Audit_Log_Of_The_Enumeration_That_Uses_It()
    {
        var outer = _target.StreamItemsOfNestedSequenceAsync();
        (await ToListAsync(outer)).ShouldBe(new[] { 1, 2 });

        _savedAuditLogs.Count.ShouldBe(2);
        _savedAuditLogs[0].Actions.ShouldHaveSingleItem().MethodName.ShouldBe(nameof(MyStreamingAuditedObject.StreamItemsOfNestedSequenceAsync));

        var actions = _savedAuditLogs[1].Actions;
        actions.Select(a => (a.MethodName, IsEnumeration(a))).ShouldBe(new[]
        {
            (nameof(MyStreamingAuditedObject.StreamItemsAsync), false),
            (nameof(MyStreamingAuditedObject.StreamItemsAsync), true),
            (nameof(MyStreamingAuditedObject.StreamItemsOfNestedSequenceAsync), true)
        });
    }

    [Fact]
    public async Task Should_Save_The_Enumeration_Audit_Log_When_The_Source_Throws_On_GetAsyncEnumerator()
    {
        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(_target.StreamItemsThrowingOnGetAsyncEnumerator()));

        _savedAuditLogs.Count.ShouldBe(2);
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
        _savedAuditLogs[1].Exceptions.ShouldContain(e => e.Message == "Failed on GetAsyncEnumerator");
    }

    [Fact]
    public async Task Should_Save_The_Enumeration_Audit_Log_When_The_Source_Throws_On_Dispose()
    {
        // Stopping early runs the finally block of the source in DisposeAsync, not in MoveNextAsync.
        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await foreach (var _ in _target.StreamItemsThrowingOnDisposeAsync())
            {
                break;
            }
        });

        _savedAuditLogs.Count.ShouldBe(2);
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
        _savedAuditLogs[1].Exceptions.ShouldContain(e => e.Message == "Failed on dispose");
    }

    [Fact]
    public async Task Should_Save_The_Enumeration_Audit_Log_After_The_Failed_Source_Has_Been_Disposed()
    {
        await Should.ThrowAsync<InvalidOperationException>(ToListAsync(_target.StreamItemsThrowingOnCurrent()));

        _savedAuditLogSnapshots.Count.ShouldBe(2);
        var (comments, actionDurations) = _savedAuditLogSnapshots[1];
        comments.ShouldBe(new[] { "Disposed the source" });
        actionDurations.ShouldHaveSingleItem().ShouldBeGreaterThanOrEqualTo(40);
        _savedAuditLogs[1].Exceptions.ShouldContain(e => e.Message == "Failed on Current");
    }

    [Fact]
    public async Task Should_Save_The_Enumeration_Audit_Log_Without_An_Action_If_Action_Info_Is_Disabled()
    {
        GetRequiredService<IOptions<AbpAuditingOptions>>().Value.DisableLogActionInfo = true;

        (await ToListAsync(_target.StreamItemsAsync(1))).ShouldBe(new[] { 1, 2 });

        _savedAuditLogs.Count.ShouldBe(2);
        _savedAuditLogs[1].Actions.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Add_The_Exception_Of_Saving_Changes_To_The_Enumeration_Audit_Log()
    {
        using (var unitOfWork = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            unitOfWork.AddDatabaseApi("Failing", new FailingSaveChangesDatabaseApi());

            (await ToListAsync(_target.StreamItemsAsync(1))).ShouldBe(new[] { 1, 2 });
        }

        _savedAuditLogs.Count.ShouldBe(2);
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
        _savedAuditLogs[1].Exceptions.ShouldContain(e => e.Message == "Failed to save changes");
    }

    [Fact]
    public async Task Should_Save_The_Exception_Thrown_When_Disposing_The_Failed_Source_In_The_Enumeration_Audit_Log()
    {
        var exception = await Should.ThrowAsync<InvalidOperationException>(ToListAsync(_target.StreamItemsThrowingOnCurrentAndDispose()));

        exception.Message.ShouldBe("Failed on dispose");
        _savedAuditLogs.Count.ShouldBe(2);
        _savedAuditLogs[1].Exceptions.ShouldContain(e => e.Message == "Failed on dispose");
    }

    [Fact]
    public async Task Should_Not_Save_An_Enumeration_Audit_Log_While_Auditing_Is_Disabled()
    {
        var sequence = _target.StreamItemsAsync(1);

        using (GetRequiredService<IAuditingHelper>().DisableAuditing())
        {
            (await ToListAsync(sequence)).ShouldBe(new[] { 1, 2 });
        }

        _savedAuditLogs.ShouldHaveSingleItem();

        (await ToListAsync(sequence)).ShouldBe(new[] { 1, 2 });

        _savedAuditLogs.Count.ShouldBe(2);
        IsEnumeration(_savedAuditLogs[1].Actions.ShouldHaveSingleItem()).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Not_Add_The_Enumeration_To_The_Current_Audit_Log_While_Auditing_Is_Disabled()
    {
        using (_auditingManager.BeginScope())
        {
            var sequence = _target.StreamItemsAsync(1);

            using (GetRequiredService<IAuditingHelper>().DisableAuditing())
            {
                (await ToListAsync(sequence)).ShouldBe(new[] { 1, 2 });
            }

            _auditingManager.Current!.Log.Actions.Select(IsEnumeration).ShouldBe(new[] { false });

            (await ToListAsync(sequence)).ShouldBe(new[] { 1, 2 });

            _auditingManager.Current!.Log.Actions.Select(IsEnumeration).ShouldBe(new[] { false, true });
        }
    }

    [Fact]
    public async Task Should_Add_The_Call_And_The_Enumeration_To_The_Current_Audit_Log()
    {
        using (var scope = _auditingManager.BeginScope())
        {
            (await ToListAsync(_target.StreamItemsAsync(50))).ShouldBe(new[] { 1, 2 });
            await Should.ThrowAsync<InvalidOperationException>(ToListAsync(_target.StreamItemsThenThrowAsync()));

            var auditLog = _auditingManager.Current!.Log;
            auditLog.Actions.Select(IsEnumeration).ShouldBe(new[] { false, true, false, true });
            auditLog.Actions[1].ExecutionDuration.ShouldBeGreaterThanOrEqualTo(80);
            auditLog.Exceptions.ShouldContain(e => e.Message == "Stream failed");

            await scope.SaveAsync();
        }

        _savedAuditLogs.ShouldHaveSingleItem();
    }

    private class FailingSaveChangesDatabaseApi : IDatabaseApi, ISupportsSavingChanges
    {
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Failed to save changes");
        }
    }

    private static bool IsEnumeration(AuditLogActionInfo action)
    {
        return action.ExtraProperties.TryGetValue(AuditingInterceptor.AsyncEnumerationPropertyName, out var value) && value is true;
    }

    private static async Task<List<int>> ToListAsync(IAsyncEnumerable<int> source)
    {
        var list = new List<int>();
        await foreach (var item in source)
        {
            list.Add(item);
        }

        return list;
    }

    public class MyStreamingAuditedObject : ITransientDependency, IAuditingEnabled
    {
        private readonly IAuditingManager _auditingManager;

        public MyStreamingAuditedObject(IAuditingManager auditingManager)
        {
            _auditingManager = auditingManager;
        }

        public virtual IAsyncEnumerable<int> StreamItemsWithCommentAsync()
        {
            _auditingManager.Current!.Log.Comments.Add("Created the sequence");
            return ProduceAsync();
        }

        public virtual IAsyncEnumerable<int> StreamItemsAfterHandledExceptionAsync()
        {
            _auditingManager.Current!.Log.Exceptions.Add(new InvalidOperationException("Handled while creating"));
            return ProduceAsync();
        }

        public virtual async IAsyncEnumerable<int> StreamItemsAsync(int delay)
        {
            await Task.Delay(delay);
            yield return 1;
            await Task.Delay(delay);
            yield return 2;
        }

        public virtual async IAsyncEnumerable<int> StreamItemsWithEntityChangeAsync()
        {
            await Task.Yield();
            _auditingManager.Current!.Log.EntityChanges.Add(new EntityChangeInfo { EntityId = "1" });
            yield return 1;
        }

        public virtual async IAsyncEnumerable<int> StreamItemsOfNestedSequenceAsync()
        {
            await foreach (var item in StreamItemsAsync(1))
            {
                yield return item;
            }
        }

        public virtual async IAsyncEnumerable<int> StreamItemsThenThrowAsync()
        {
            await Task.Yield();
            yield return 1;
            throw new InvalidOperationException("Stream failed");
        }

        public virtual IAsyncEnumerable<int> StreamItemsThrowingOnCurrent()
        {
            return new ThrowingOnCurrentSequence(_auditingManager);
        }

        public virtual IAsyncEnumerable<int> StreamItemsThrowingOnCurrentAndDispose()
        {
            return new ThrowingOnCurrentAndDisposeSequence();
        }

        public virtual IAsyncEnumerable<int> StreamItemsThrowingOnGetAsyncEnumerator()
        {
            return new ThrowingOnGetAsyncEnumeratorSequence();
        }

        public virtual async IAsyncEnumerable<int> StreamItemsThrowingOnDisposeAsync()
        {
            try
            {
                await Task.Yield();
                yield return 1;
            }
            finally
            {
                throw new InvalidOperationException("Failed on dispose");
            }
        }

        private static async IAsyncEnumerable<int> ProduceAsync()
        {
            await Task.Yield();
            yield return 1;
            yield return 2;
        }

        private class ThrowingOnCurrentSequence : IAsyncEnumerable<int>, IAsyncEnumerator<int>
        {
            private readonly IAuditingManager _auditingManager;

            public ThrowingOnCurrentSequence(IAuditingManager auditingManager)
            {
                _auditingManager = auditingManager;
            }

            public int Current => throw new InvalidOperationException("Failed on Current");

            public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return this;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(true);
            }

            public async ValueTask DisposeAsync()
            {
                await Task.Delay(50);
                _auditingManager.Current!.Log.Comments.Add("Disposed the source");
            }
        }

        private class ThrowingOnCurrentAndDisposeSequence : IAsyncEnumerable<int>, IAsyncEnumerator<int>
        {
            public int Current => throw new InvalidOperationException("Failed on Current");

            public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return this;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(true);
            }

            public ValueTask DisposeAsync()
            {
                throw new InvalidOperationException("Failed on dispose");
            }
        }

        private class ThrowingOnGetAsyncEnumeratorSequence : IAsyncEnumerable<int>
        {
            public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                throw new InvalidOperationException("Failed on GetAsyncEnumerator");
            }
        }
    }
}
