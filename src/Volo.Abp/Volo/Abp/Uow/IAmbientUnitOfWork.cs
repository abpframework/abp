using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    public interface IAmbientUnitOfWork : IUnitOfWorkAccessor
    {
        void SetUnitOfWork([CanBeNull] IUnitOfWork unitOfWork);
    }
}