using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    /// <summary>
    /// Base class that can be used to implement <see cref="IBackgroundWorker"/>.
    /// </summary>
    public abstract class BackgroundWorkerBase : RunnableBase, IBackgroundWorker
    {
        //TODO: Add UOW, Localization and other useful properties..?

        public ILogger<BackgroundWorkerBase> Logger { protected get; set; }

        protected BackgroundWorkerBase()
        {
            Logger = NullLogger<BackgroundWorkerBase>.Instance;
        }

        public override void Start()
        {
            Logger.LogDebug("Starting background worker: " + ToString());
            base.Start();
            Logger.LogDebug("Started background worker: " + ToString());
        }

        public override void Stop()
        {
            Logger.LogDebug("Stopping background worker: " + ToString());
            base.Stop();
            Logger.LogDebug("Stopped background worker: " + ToString());
        }

        public override void WaitToStop()
        {
            Logger.LogDebug("Waiting background worker to completely stop: " + ToString());
            base.WaitToStop();
            Logger.LogDebug("Background worker is completely stopped: " + ToString());
        }
        
        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}