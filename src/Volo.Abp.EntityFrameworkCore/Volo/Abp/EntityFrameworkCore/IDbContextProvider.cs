namespace Volo.Abp.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        TDbContext GetDbContext();
    }
}