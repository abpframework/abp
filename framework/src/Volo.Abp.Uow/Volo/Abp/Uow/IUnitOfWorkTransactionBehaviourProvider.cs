namespace Volo.Abp.Uow
{
    public interface IUnitOfWorkTransactionBehaviourProvider
    {
        bool? IsTransactional { get; }
    }
}