namespace Volo.Abp.Uow;

public interface IUnitOfWorkAccessor
{
    IUnitOfWork? UnitOfWork { get; }

    void SetUnitOfWork(IUnitOfWork? unitOfWork);
}
