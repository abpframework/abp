using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IBasicUnitOfWork Begin([NotNull] UnitOfWorkStartOptions options, bool requiresNew = false);

        [NotNull]
        IBasicUnitOfWork Reserve([NotNull] string reservationName, bool requiresNew = false);

        void BeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkStartOptions options);

        bool TryBeginReserved([NotNull] string reservationName, [NotNull] UnitOfWorkStartOptions options);
    }
}