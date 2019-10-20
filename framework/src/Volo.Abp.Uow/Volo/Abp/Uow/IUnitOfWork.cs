using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWork : IDatabaseApiContainer, ITransactionApiContainer, IDisposable
    {
        Guid Id { get; }

        //TODO: Switch to OnFailed (sync) and OnDisposed (sync) methods to be compatible with OnCompleted
        event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        event EventHandler<UnitOfWorkEventArgs> Disposed;

        IAbpUnitOfWorkOptions Options { get; }

        IUnitOfWork Outer { get; }

        bool IsReserved { get; }

        bool IsDisposed { get; }

        bool IsCompleted { get; }

        string ReservationName { get; }

        void SetOuter([CanBeNull] IUnitOfWork outer);

        void Initialize([NotNull] AbpUnitOfWorkOptions options);

        void Reserve([NotNull] string reservationName);

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        void Complete();

        Task CompleteAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);

        void OnCompleted(Func<Task> handler);
    }
}
