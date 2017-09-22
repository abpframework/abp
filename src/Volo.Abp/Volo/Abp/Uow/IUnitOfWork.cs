using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IUnitOfWork : IBasicUnitOfWork, IDatabaseApiContainer
    {
        IUnitOfWork Outer { get; }

        bool IsReserved { get; set; }

        string ReservationName { get; set; }

        void SetOuter([CanBeNull] IUnitOfWork outer);
    }
}
