using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWork : IUnitOfWork, ITransientDependency
    {
        public Guid Id { get; } = Guid.NewGuid();

        public IUnitOfWorkOptions Options { get; private set; }

        public IUnitOfWork Outer { get; private set; }

        public bool IsReserved { get; set; }

        public string ReservationName { get; set; }

        public event EventHandler Completed;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        public event EventHandler Disposed;

        public IServiceProvider ServiceProvider { get; }

        private readonly Dictionary<string, IDatabaseApi> _databaseApis;
        private readonly Dictionary<string, ITransactionApi> _transactionApis;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        private Exception _exception;
        private bool _isCompleted;
        private bool _isDisposed;

        public UnitOfWork(IServiceProvider serviceProvider, IOptions<UnitOfWorkDefaultOptions> options)
        {
            ServiceProvider = serviceProvider;
            _defaultOptions = options.Value;

            _databaseApis = new Dictionary<string, IDatabaseApi>();
            _transactionApis = new Dictionary<string, ITransactionApi>();
        }

        public void Initialize(UnitOfWorkOptions options)
        {
            Check.NotNull(options, nameof(options));

            if (Options != null)
            {
                throw new AbpException("This unit of work is already initialized before!");
            }

            Options = _defaultOptions.Normalize(options.Clone());
        }

        public void SetOuter(IUnitOfWork outer)
        {
            Outer = outer;
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
                CommitTransactions();
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
                await CommitTransactionsAsync();
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        public IDatabaseApi FindDatabaseApi(string key)
        {
            return _databaseApis.GetOrDefault(key);
        }

        public void AddDatabaseApi(string key, IDatabaseApi api)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(api, nameof(api));

            if (_databaseApis.ContainsKey(key))
            {
                throw new AbpException("There is already a database API in this unit of work with given key: " + key);
            }

            _databaseApis.Add(key, api);
        }

        public IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(factory, nameof(factory));

            return _databaseApis.GetOrAdd(key, factory);
        }

        public ITransactionApi FindTransactionApi(string key)
        {
            Check.NotNull(key, nameof(key));

            return _transactionApis.GetOrDefault(key);
        }

        public void AddTransactionApi(string key, ITransactionApi api)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(api, nameof(api));

            if (_transactionApis.ContainsKey(key))
            {
                throw new AbpException("There is already a transaction API in this unit of work with given key: " + key);
            }

            _transactionApis.Add(key, api);
        }

        public ITransactionApi GetOrAddTransactionApi(string key, Func<ITransactionApi> factory)
        {
            Check.NotNull(key, nameof(key));
            Check.NotNull(factory, nameof(factory));

            return _transactionApis.GetOrAdd(key, factory);
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

        private void PreventMultipleComplete()
        {
            if (_isCompleted)
            {
                throw new AbpException("Complete is called before!");
            }

            _isCompleted = true;
        }

        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }

        protected virtual void CommitTransactions()
        {
            foreach (var transaction in _transactionApis.Values)
            {
                transaction.Commit();
            }
        }

        protected virtual async Task CommitTransactionsAsync()
        {
            foreach (var transaction in _transactionApis.Values)
            {
                await transaction.CommitAsync();
            }
        }
    }
}