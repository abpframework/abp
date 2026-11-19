using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.DynamicProxy;

public class AbpMethodInvocationAsyncEnumerableExtensions_Tests
{
    private static readonly AsyncLocal<string?> ScopeValue = new();

    [Fact]
    public async Task Should_Keep_The_Scope_When_The_Enumeration_Is_Started_With_A_Suppressed_Flow()
    {
        var invocation = new TestMethodInvocation(typeof(AbpMethodInvocationAsyncEnumerableExtensions_Tests).GetMethod(nameof(GetScopeValuesAsync))!)
        {
            ReturnValue = GetScopeValuesAsync()
        };

        invocation.WrapAsyncEnumerable(() =>
        {
            ScopeValue.Value = "in-scope";
            return new AbpInterceptionScope();
        }).ShouldBeTrue();

        var enumerator = ((IAsyncEnumerable<string?>)invocation.ReturnValue).GetAsyncEnumerator();
        ValueTask<bool> moveNext;
        using (ExecutionContext.SuppressFlow())
        {
            moveNext = enumerator.MoveNextAsync();

            ExecutionContext.IsFlowSuppressed().ShouldBeTrue();
            ScopeValue.Value.ShouldBeNull();
        }

        var values = new List<string?>();
        while (await moveNext)
        {
            values.Add(enumerator.Current);
            moveNext = enumerator.MoveNextAsync();
        }

        await enumerator.DisposeAsync();

        values.ShouldBe(new[] { "in-scope", "in-scope" });
        ScopeValue.Value.ShouldBeNull();
    }

    public static async IAsyncEnumerable<string?> GetScopeValuesAsync()
    {
        await Task.Delay(1);
        yield return ScopeValue.Value;
        await Task.Delay(1);
        yield return ScopeValue.Value;
    }

    private class TestMethodInvocation : IAbpMethodInvocation
    {
        public TestMethodInvocation(MethodInfo method)
        {
            Method = method;
        }

        public object?[] Arguments => Array.Empty<object?>();

        public IReadOnlyDictionary<string, object?> ArgumentsDictionary => new Dictionary<string, object?>();

        public Type[]? GenericArguments => null;

        public object? TargetObject => null;

        public MethodInfo Method { get; }

        public object ReturnValue { get; set; } = default!;

        public Task ProceedAsync()
        {
            return Task.CompletedTask;
        }
    }
}
