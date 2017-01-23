using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    //Find a better naming :(
    public interface IBasicUnitOfWork : IDisposable
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Complete();

        Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}