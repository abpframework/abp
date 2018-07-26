using JetBrains.Annotations;

namespace Volo.Abp.Threading
{
    public static class RunnableExtensions
    {
        public static void Start([NotNull] this IRunnable runnable)
        {
            Check.NotNull(runnable, nameof(runnable));

            AsyncHelper.RunSync(() => runnable.StartAsync());
        }

        public static void Stop([NotNull] this IRunnable runnable)
        {
            Check.NotNull(runnable, nameof(runnable));

            AsyncHelper.RunSync(() => runnable.StopAsync());
        }
    }
}