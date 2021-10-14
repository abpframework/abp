using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Boxes
{
    internal static class TaskDelayHelper
    {
        public static async Task DelayAsync(int milliseconds, CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Delay(milliseconds, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }
}