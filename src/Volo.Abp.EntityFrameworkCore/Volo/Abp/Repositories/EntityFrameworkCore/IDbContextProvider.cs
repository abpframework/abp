using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        TDbContext GetDbContext();
    }
}