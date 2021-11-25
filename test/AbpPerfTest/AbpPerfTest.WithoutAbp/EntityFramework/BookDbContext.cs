using AbpPerfTest.WithoutAbp.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbpPerfTest.WithoutAbp.EntityFramework
{
    public class BookDbContext : DbContext
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
