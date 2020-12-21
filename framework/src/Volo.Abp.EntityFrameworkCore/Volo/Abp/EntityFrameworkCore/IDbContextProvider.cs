using System.Threading.Tasks;

namespace Volo.Abp.EntityFrameworkCore
{
    public interface IDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        TDbContext GetDbContext();

        Task<TDbContext> GetDbContextAsync();
    }
}
