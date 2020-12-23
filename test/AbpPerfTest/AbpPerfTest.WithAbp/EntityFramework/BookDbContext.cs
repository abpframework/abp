using AbpPerfTest.WithAbp.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AbpPerfTest.WithAbp.EntityFramework
{
    public class BookDbContext : AbpDbContext<BookDbContext>
    {
        public DbSet<Book> Books { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> builderOptions)
            : base(builderOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(b =>
            {
                b.ToTable("Books");
                b.Property(x => x.Name).HasMaxLength(128);
            });
        }
    }
}
