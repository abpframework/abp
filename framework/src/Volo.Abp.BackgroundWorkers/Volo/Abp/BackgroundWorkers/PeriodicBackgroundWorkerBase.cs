using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    /// <summary>
    /// Extends <see cref="BackgroundWorkerBase"/> to add a periodic running Timer. 
    /// </summary>
    public abstract class PeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        protected readonly AbpTimer Timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicBackgroundWorkerBase"/> class.
        /// </summary>
        /// <param name="timer">A timer.</param>
        protected PeriodicBackgroundWorkerBase(AbpTimer timer)
        {
            Timer = timer;
            Timer.Elapsed += Timer_Elapsed;
        }

        public override async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await base.StartAsync(cancellationToken);
            Timer.Start(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken = default)
        {
            Timer.Stop(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
        
        private void Timer_Elapsed(object sender, System.EventArgs e)
        {
            try
            {
                DoWork();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// Periodic works should be done by implementing this method.
        /// </summary>
        protected abstract void DoWork();
    }
}