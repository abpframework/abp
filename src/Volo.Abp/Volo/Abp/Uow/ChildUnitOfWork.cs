using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    internal class ChildUnitOfWork : IUnitOfWork
    {
        public IServiceProvider ServiceProvider => _parent.ServiceProvider;

        private readonly IUnitOfWork _parent;

        public ChildUnitOfWork([NotNull] IUnitOfWork parent)
        {
            Check.NotNull(parent, nameof(parent));

            _parent = parent;
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
            _parent.Complete();
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

        public IDatabaseApi AddDatabaseApi(string id, IDatabaseApi databaseApi)
        {
            return _parent.AddDatabaseApi(id, databaseApi);
        }



        public void Dispose()
        {

        }
    }
}