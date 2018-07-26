using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Volo.Abp.BackgroundWorkers
{
    /// <summary>
    /// Base class that can be used to implement <see cref="IBackgroundWorker"/>.
    /// </summary>
    public abstract class BackgroundWorkerBase : IBackgroundWorker
    {
        //TODO: Add UOW, Localization and other useful properties..?

        public ILogger<BackgroundWorkerBase> Logger { protected get; set; }

        protected BackgroundWorkerBase()
        {
            Logger = NullLogger<BackgroundWorkerBase>.Instance;
        }
        
        public virtual Task StartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("Started background worker: " + ToString());
            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("Stopped background worker: " + ToString());
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}