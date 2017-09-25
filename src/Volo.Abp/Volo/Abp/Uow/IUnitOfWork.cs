using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWork : IBasicUnitOfWork, IDatabaseApiContainer, ITransactionApiContainer
    {
        IUnitOfWorkStartOptions Options { get; }

        IUnitOfWork Outer { get; }

        bool IsReserved { get; set; }

        string ReservationName { get; set; }

        void SetOuter([CanBeNull] IUnitOfWork outer);

        void SetOptions(UnitOfWorkStartOptions options);
    }
}
