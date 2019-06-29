using System;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Volo.Abp.Threading
{
    /// <summary>
    /// This class is used to ensure running of a code block only once.
    /// It can be instantiated as a static object to ensure that the code block runs only once in the application lifetime.
    /// </summary>
    public class AsyncOneTimeRunner
    {
        private volatile bool _runBefore;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task RunAsync(Func<Task> action)
        {
            if (_runBefore)
            {
                return;
            }

            using (await _semaphore.LockAsync())
            {
                if (_runBefore)
                {
                    return;
                }

                await action();

                _runBefore = true;
            }
        }
    }
}