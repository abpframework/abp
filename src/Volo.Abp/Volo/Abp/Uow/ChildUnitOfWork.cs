using System;
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

        public Task SaveChangesAsync()
        {
            return _parent.SaveChangesAsync();
        }

        public Task CompleteAsync()
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