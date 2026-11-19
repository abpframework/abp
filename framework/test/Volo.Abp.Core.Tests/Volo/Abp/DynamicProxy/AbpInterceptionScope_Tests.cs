using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.DynamicProxy;

public class AbpInterceptionScope_Tests
{
    [Fact]
    public async Task Should_Dispose_The_Given_Object_When_Disposed()
    {
        var disposable = new TestDisposable();
        var scope = new AbpInterceptionScope(disposable);

        await scope.CompleteAsync(CancellationToken.None);
        await scope.FailAsync(new InvalidOperationException());
        disposable.IsDisposed.ShouldBeFalse();

        scope.Dispose();
        disposable.IsDisposed.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Work_Without_A_Disposable()
    {
        var scope = new AbpInterceptionScope();

        await scope.CompleteAsync(CancellationToken.None);
        await scope.FailAsync(new InvalidOperationException());
        scope.Dispose();
    }

    private class TestDisposable : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
