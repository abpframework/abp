namespace Volo.Abp.Threading
{
    /// <summary>
    /// Some extension methods for <see cref="IRunnable"/>.
    /// </summary>
    public static class RunnableExtensions
    {
        /// <summary>
        /// Calls <see cref="IRunnable.Stop"/> and then <see cref="IRunnable.WaitToStop"/>.
        /// </summary>
        public static void StopAndWaitToStop(this IRunnable runnable)
        {
            runnable.Stop();
            runnable.WaitToStop();
        }
    }
}