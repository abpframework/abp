using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.ExtensionMethods;

namespace Volo.Abp.Uow
{
    internal class ChildUnitOfWork : IUnitOfWork
    {
        public event EventHandler Completed;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;
        public event EventHandler Disposed;

        public IServiceProvider ServiceProvider => _parent.ServiceProvider;

        private readonly IUnitOfWork _parent;

        public ChildUnitOfWork([NotNull] IUnitOfWork parent)
        {
            Check.NotNull(parent, nameof(parent));

            _parent = parent;

            _parent.Completed += (sender, args) => { Completed.InvokeSafely(sender, args); };
            _parent.Failed += (sender, args) => { Failed.InvokeSafely(sender, args); };
            _parent.Disposed += (sender, args) => { Disposed.InvokeSafely(sender, args); };
        }

        public void SaveChanges()
        {
            _parent.SaveChanges();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _parent.SaveChangesAsync(cancellationToken);
        }
        
        public void Complete()
        {
            
        }

        public Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
        
        public IDatabaseApi FindDatabaseApi(string id)
        {
            return _parent.FindDatabaseApi(id);
        }

        public IDatabaseApi GetOrAddDatabaseApi(string id, Func<IDatabaseApi> factory)
        {
            return _parent.GetOrAddDatabaseApi(id, factory);
        }
        
        public void Dispose()
        {

        }
    }
}