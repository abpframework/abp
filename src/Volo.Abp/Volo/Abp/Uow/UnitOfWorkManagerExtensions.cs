using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public static class UnitOfWorkManagerExtensions
    {
        [NotNull]
        public static IBasicUnitOfWork Begin([NotNull] this IUnitOfWorkManager unitOfWorkManager, bool requiresNew = false)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));

            return unitOfWorkManager.Begin(new UnitOfWorkOptions(), requiresNew);
        }

        public static void BeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.BeginReserved(reservationName, new UnitOfWorkOptions());
        }

        public static void TryBeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.TryBeginReserved(reservationName, new UnitOfWorkOptions());
        }
    }
}