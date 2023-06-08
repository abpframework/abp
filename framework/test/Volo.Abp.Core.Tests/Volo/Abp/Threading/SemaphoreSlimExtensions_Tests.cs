using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Threading;

public class SemaphoreSlimExtensions_Tests
{
    [Fact]
    public async Task LockAsync_Test()
    {
        var semaphore = new SemaphoreSlim(0, 1);

        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            await semaphore.LockAsync(10);
        });

        semaphore = new SemaphoreSlim(1, 1);
        using (await semaphore.LockAsync())
        {
            semaphore.CurrentCount.ShouldBe(0);
        }
        semaphore.CurrentCount.ShouldBe(1);
    }
}
