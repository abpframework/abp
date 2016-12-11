using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        public AbpDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {

        }
    }
}
