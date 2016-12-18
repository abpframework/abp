using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWork : IDisposable, IServiceProviderAccessor, ITransientDependency
    {
        [CanBeNull]
        IDatabaseApi FindDatabaseApi([NotNull] string id);

        [NotNull]
        IDatabaseApi GetOrAddDatabaseApi(string id, Func<IDatabaseApi> factory);

        IDatabaseApi AddDatabaseApi(string id, IDatabaseApi databaseApi);

        Task SaveChangesAsync();

        Task CompleteAsync();
    }
}
