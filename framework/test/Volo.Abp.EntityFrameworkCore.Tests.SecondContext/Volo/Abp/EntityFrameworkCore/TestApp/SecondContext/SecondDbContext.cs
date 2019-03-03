using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhoneInSecondDbContext>(b =>
            {
                b.HasKey(p => new { p.PersonId, p.Number });
            });
        }
    }
}