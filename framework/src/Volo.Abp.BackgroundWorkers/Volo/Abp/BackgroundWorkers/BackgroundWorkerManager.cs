using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    /// <summary>
    /// Implements <see cref="IBackgroundWorkerManager"/>.
    /// </summary>
    public class BackgroundWorkerManager : RunnableBase, IBackgroundWorkerManager, ISingletonDependency, IDisposable
    {
        private readonly List<IBackgroundWorker> _backgroundWorkers;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkerManager"/> class.
        /// </summary>
        public BackgroundWorkerManager()
        {
            _backgroundWorkers = new List<IBackgroundWorker>();
        }

        public override void Start()
        {
            base.Start();

            _backgroundWorkers.ForEach(worker => worker.Start());
        }

        public override void Stop()
        {
            _backgroundWorkers.ForEach(worker => worker.Stop());

            base.Stop();
        }

        public override void WaitToStop()
        {
            _backgroundWorkers.ForEach(worker => worker.WaitToStop());

            base.WaitToStop();
        }

        public void Add(IBackgroundWorker worker)
        {
            _backgroundWorkers.Add(worker);

            if (IsRunning)
            {
                worker.Start();
            }
        }

        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            //TODO: ???
        }
    }
}
