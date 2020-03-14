using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);
        private ILoggerFactory _loggerFactory;

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

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