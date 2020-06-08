namespace Volo.Abp.EntityFrameworkCore
{
    public interface IAbpEfCoreDbContext : IEfCoreDbContext
    {
        void Initialize(AbpEfCoreDbContextInitializationContext initializationContext);
    }
}