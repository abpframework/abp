using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;

namespace Volo.Abp.EntityFrameworkCore.TestApp.FourthContext
{
    /* This dbcontext is just for testing to replace dbcontext from the application using ReplaceDbContextAttribute
     */
    public class FourthDbContext : AbpDbContext<FourthDbContext>, IFourthDbContext
    {
        public DbSet<FourthDbContextDummyEntity> FourthDummyEntities { get; set; }

        public FourthDbContext(DbContextOptions<FourthDbContext> options)
            : base(options)
        {
        }
    }
}
