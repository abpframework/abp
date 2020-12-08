using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.Threading
{
    /// <summary>
    /// A robust timer implementation that ensures no overlapping occurs. It waits exactly specified <see cref="Period"/> between ticks.
    /// </summary>
    public class AbpAsyncTimer : ITransientDependency
    {
        /// <summary>
        /// This func is raised periodically according to Period of Timer.
        /// </summary>
        public Func<AbpAsyncTimer, Task> Elapsed = _ => Task.CompletedTask;

        /// <summary>
        /// Task period of timer (as milliseconds).
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Indicates whether timer raises Elapsed event on Start method of Timer for once.
        /// Default: False.
        /// </summary>
        public bool RunOnStart { get; set; }

        public ILogger<AbpAsyncTimer> Logger { get; set; }

        public IExceptionNotifier ExceptionNotifier { get; set; }

        private readonly Timer _taskTimer;
        private volatile bool _performingTasks;
        private volatile bool _isRunning;

        public AbpAsyncTimer()
        {
            ExceptionNotifier = NullExceptionNotifier.Instance;
            Logger = NullLogger<AbpAsyncTimer>.Instance;

            _taskTimer = new Timer(
                TimerCallBack,
                null,
                Timeout.Infinite,
                Timeout.Infinite
            );
        }

        public void Start(CancellationToken cancellationToken = default)
        {
            if (Period <= 0)
            {
                throw new AbpException("Period should be set before starting the timer!");
            }

            lock (_taskTimer)
            {
                _taskTimer.Change(RunOnStart ? 0 : Period, Timeout.Infinite);
                _isRunning = true;
            }
        }

        public void Stop(CancellationToken cancellationToken = default)
        {
            lock (_taskTimer)
            {
                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
                while (_performingTasks)
                {
                    Monitor.Wait(_taskTimer);
                }

                _isRunning = false;
            }
        }

        /// <summary>
        /// This method is called by _taskTimer.
        /// </summary>
        /// <param name="state">Not used argument</param>
        private void TimerCallBack(object state)
        {
            lock (_taskTimer)
            {
                if (!_isRunning || _performingTasks)
                {
                    return;
                }

                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _performingTasks = true;
            }

            _ = Timer_Elapsed();
        }

        private async Task Timer_Elapsed()
        {
            try
            {
                await Elapsed(this);
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
                await ExceptionNotifier.NotifyAsync(ex);
            }
            finally
            {
                lock (_taskTimer)
                {
                    _performingTasks = false;
                    if (_isRunning)
                    {
                        _taskTimer.Change(Period, Timeout.Infinite);
                    }

                    Monitor.Pulse(_taskTimer);
                }
            }
        }
    }
}
