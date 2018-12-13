using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.EntityFrameworkCore
{
    public class TestAppDbContext : AbpDbContext<TestAppDbContext>, IThirdDbContext
    {
        public DbSet<Person> People { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }

        public TestAppDbContext(DbContextOptions<TestAppDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(b =>
            {
                b.ConfigureExtraProperties();
            });

            modelBuilder.Entity<Phone>(b =>
            {
                b.HasKey(p => new {p.PersonId, p.Number});
            });

            modelBuilder.Entity<City>(b =>
            {
                b.ConfigureExtraProperties();
            });

            modelBuilder.Entity<ThirdDbContextDummyEntity>(b =>
            {
                b.ConfigureExtraProperties();
            });
        }
    }
}
