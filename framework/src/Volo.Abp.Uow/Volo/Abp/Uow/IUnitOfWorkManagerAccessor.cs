namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkManagerAccessor
    {
        IUnitOfWorkManager UnitOfWorkManager { get; }
    }
}
