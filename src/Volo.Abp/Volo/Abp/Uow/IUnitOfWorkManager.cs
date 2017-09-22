using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        [CanBeNull]
        IUnitOfWork Current { get; }

        [NotNull]
        IBasicUnitOfWork Begin([NotNull] UnitOfWorkStartOptions options);

        [NotNull]
        IBasicUnitOfWork BeginReserved([NotNull] string reservationName);
    }
}