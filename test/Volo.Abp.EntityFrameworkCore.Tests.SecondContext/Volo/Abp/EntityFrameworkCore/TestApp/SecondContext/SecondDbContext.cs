using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    public class SecondDbContext : AbpDbContext<SecondDbContext>
    {
        public DbSet<BookInSecondDbContext> Books { get; set; }

        public DbSet<PhoneInSecondDbContext> Phones { get; set; }

        public SecondDbContext(DbContextOptions<SecondDbContext> options) 
            : base(options)
        {
        }
    }
}