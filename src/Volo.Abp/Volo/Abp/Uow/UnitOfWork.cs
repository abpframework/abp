using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWork : IUnitOfWork, ITransientDependency
    {
        public event EventHandler Completed;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        public event EventHandler Disposed;

        public IServiceProvider ServiceProvider { get; }

        private readonly Dictionary<string, IDatabaseApi> _databaseApis;

        private Exception _exception;
        private bool _isCompleted;
        private bool _isDisposed;

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            _databaseApis = new Dictionary<string, IDatabaseApi>();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (!_isCompleted || _exception != null)
            {
                OnFailed(_exception);
            }

            OnDisposed();
        }

        public void SaveChanges()
        {
            foreach (var databaseApi in _databaseApis.Values)
            {
                (databaseApi as ISupportsSavingChanges)?.SaveChanges();
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var databaseApi in _databaseApis.Values)
            {
                if (databaseApi is ISupportsSavingChanges)
                {
                    await (databaseApi as ISupportsSavingChanges).SaveChangesAsync(cancellationToken);
                }
            }
        }

        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                SaveChanges();
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            PreventMultipleComplete();

            try
            {
                await SaveChangesAsync(cancellationToken);
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public IDatabaseApi FindDatabaseApi(string id)
        {
            return _databaseApis.GetOrDefault(id);
        }

        public IDatabaseApi GetOrAddDatabaseApi(string id, Func<IDatabaseApi> factory)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNull(factory, nameof(factory));

            return _databaseApis.GetOrAdd(id, factory);
        }

        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleted)
            {
                throw new AbpException("Complete is called before!");
            }

            _isCompleted = true;
        }
    }
}