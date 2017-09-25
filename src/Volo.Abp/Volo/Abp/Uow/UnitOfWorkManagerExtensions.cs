using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public static class UnitOfWorkManagerExtensions
    {
        [NotNull]
        public static IBasicUnitOfWork Begin([NotNull] this IUnitOfWorkManager unitOfWorkManager)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));

            return unitOfWorkManager.Begin(new UnitOfWorkStartOptions());
        }

        [NotNull]
        public static IBasicUnitOfWork BeginNew([NotNull] this IUnitOfWorkManager unitOfWorkManager)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));

            return unitOfWorkManager.Begin(new UnitOfWorkStartOptions {RequiresNew = true});
        }

        [NotNull]
        public static IBasicUnitOfWork Reserve([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            return unitOfWorkManager.Begin(new UnitOfWorkStartOptions { ReservationName = reservationName });
        }

        public static void BeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.BeginReserved(reservationName, new UnitOfWorkStartOptions());
        }

        public static void TryBeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.TryBeginReserved(reservationName, new UnitOfWorkStartOptions());
        }
    }
}