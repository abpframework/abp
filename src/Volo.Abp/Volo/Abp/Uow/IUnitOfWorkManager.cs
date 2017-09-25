using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IBasicUnitOfWork Begin([NotNull] UnitOfWorkStartOptions options);

        void BeginReserved([NotNull] string reservationName);

        bool TryBeginReserved([NotNull] string reservationName);
    }
}