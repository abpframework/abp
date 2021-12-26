using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Threading;

public class NullCancellationTokenProvider_Tests : AbpIntegratedTest<AbpThreadingTestModule>
{
    private readonly ICancellationTokenProvider _cancellationTokenProvider;

    public NullCancellationTokenProvider_Tests()
    {
        _cancellationTokenProvider = NullCancellationTokenProvider.Instance;
    }

    [Fact]
    public void Should_Return_None_Token()
    {
        _cancellationTokenProvider.Token.ShouldBe(CancellationToken.None);
    }

    [Fact]
    public void Should_Return_Specific_Token()
    {
        var cts = new CancellationTokenSource();

        using (_cancellationTokenProvider.Use(cts.Token))
        {
            var newCancellationTokenProvider = NullCancellationTokenProvider.Instance;

            newCancellationTokenProvider.Token.ShouldBe(cts.Token);
        }

        _cancellationTokenProvider.Token.ShouldBe(CancellationToken.None);
    }

    [Fact]
    public void Should_Cancel_After_100_Milliseconds()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(100));

        using (_cancellationTokenProvider.Use(cts.Token))
        {
            var newCancellationTokenProvider = NullCancellationTokenProvider.Instance;
            Should.Throw<OperationCanceledException>(() => LongTask(1000, newCancellationTokenProvider.Token));
        }
    }

    private void LongTask(int loopCounter, CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < loopCounter; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Thread.Sleep(10);
        }
    }
}
