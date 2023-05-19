using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Shouldly;
using Xunit;

namespace Volo.Abp.Threading;

public class AsyncHelper_Tests
{
    [Fact]
    public void RunSync_Test_Without_Return_Value()
    {
        AsyncHelper.RunSync(MyTaskWithoutReturnValueAsync);
    }

    [Fact]
    public void RunSync_Test_With_Return_Value()
    {
        AsyncHelper.RunSync(() => MyTaskWithReturnValueAsync(42)).ShouldBe(42);
    }

    [Fact]
    public void IsAsync_Should_Work()
    {
        GetType().GetMethod(
            "MyTaskWithoutReturnValueAsync",
            BindingFlags.NonPublic | BindingFlags.Static
        ).IsAsync().ShouldBe(true);

        GetType().GetMethod(
            "MyTaskWithReturnValueAsync",
            BindingFlags.NonPublic | BindingFlags.Static
        ).IsAsync().ShouldBe(true);

        GetType().GetMethod(
            "MyTaskWithReturnValue2",
            BindingFlags.NonPublic | BindingFlags.Instance
        ).IsAsync().ShouldBe(false);
    }

    private static async Task MyTaskWithoutReturnValueAsync()
    {
        await Task.Delay(1);
    }

    private static async Task<int> MyTaskWithReturnValueAsync(int aNumber)
    {
        await Task.Delay(1);
        return aNumber;
    }

    [UsedImplicitly]
    private int MyTaskWithReturnValue2(int aNumber)
    {
        return aNumber;
    }
}
