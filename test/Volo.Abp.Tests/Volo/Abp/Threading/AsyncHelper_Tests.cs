using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Threading
{
    public class AsyncHelper_Tests
    {
        [Fact]
        public void RunSync_Test_Without_Return_Value()
        {
            AsyncHelper.RunSync(MyTaskWithoutReturnValue);
        }

        [Fact]
        public void RunSync_Test_With_Return_Value()
        {
            AsyncHelper.RunSync(() => MyTaskWithReturnValue(42)).ShouldBe(42);
        }

        private static async Task MyTaskWithoutReturnValue()
        {
            await Task.Delay(1);
        }

        private static async Task<int> MyTaskWithReturnValue(int aNumber)
        {
            await Task.Delay(1);
            return aNumber;
        }
    }
}
