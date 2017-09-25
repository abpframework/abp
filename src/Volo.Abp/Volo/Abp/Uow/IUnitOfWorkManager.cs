using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IBasicUnitOfWork Begin([NotNull] UnitOfWorkStartOptions options);

        void BeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkStartOptions options);

        bool TryBeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkStartOptions options);
    }
}