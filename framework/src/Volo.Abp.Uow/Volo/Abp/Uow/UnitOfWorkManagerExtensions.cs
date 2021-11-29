using System;
using System.Data;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public static class UnitOfWorkManagerExtensions
    {
        [NotNull]
        public static IUnitOfWork Begin(
            [NotNull] this IUnitOfWorkManager unitOfWorkManager,
            bool requiresNew = false,
            bool isTransactional = false,
            IsolationLevel? isolationLevel = null,
            int? timeout = null)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));

            return unitOfWorkManager.Begin(new AbpUnitOfWorkOptions
            {
                IsTransactional = isTransactional,
                IsolationLevel = isolationLevel,
                Timeout = timeout
            }, requiresNew);
        }

        public static void BeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.BeginReserved(reservationName, new AbpUnitOfWorkOptions());
        }

        public static void TryBeginReserved([NotNull] this IUnitOfWorkManager unitOfWorkManager, [NotNull] string reservationName)
        {
            Check.NotNull(unitOfWorkManager, nameof(unitOfWorkManager));
            Check.NotNull(reservationName, nameof(reservationName));

            unitOfWorkManager.TryBeginReserved(reservationName, new AbpUnitOfWorkOptions());
        }
    }
}
