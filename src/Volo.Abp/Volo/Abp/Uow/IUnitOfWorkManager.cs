namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManager
    {
        IUnitOfWork Current { get; }

        IUnitOfWork Begin();
    }
}