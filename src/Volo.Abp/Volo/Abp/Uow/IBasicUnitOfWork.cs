using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    //TODO: Find a better naming :(
    public interface IBasicUnitOfWork : IDisposable
    {
        Guid Id { get; }

        event EventHandler Completed;

        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        event EventHandler Disposed;

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Complete();

        Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}