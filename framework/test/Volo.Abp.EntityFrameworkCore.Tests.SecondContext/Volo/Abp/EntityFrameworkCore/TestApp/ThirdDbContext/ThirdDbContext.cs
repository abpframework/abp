using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;

/* This dbcontext is just for testing to replace dbcontext from the application using AbpDbContextRegistrationOptions.ReplaceDbContext
 */
public class ThirdDbContext : AbpDbContext<ThirdDbContext>, IThirdDbContext
{
    public DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }

    public ThirdDbContext(DbContextOptions<ThirdDbContext> options)
        : base(options)
    {
    }
}
