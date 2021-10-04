namespace Volo.Abp.Uow
{
    public interface IAmbientUnitOfWork : IUnitOfWorkAccessor
    {
        IUnitOfWork GetCurrentByChecking();
    }
}