using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.EntityFrameworkCore
{
    public class TestAppDbContext : AbpDbContext<TestAppDbContext>, IThirdDbContext
    {
        public DbSet<Person> People { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<PersonView> PersonView { get; set; }

        public DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }

        public DbSet<EntityWithIntPk> EntityWithIntPks { get; set; }
        
        public DbSet<Author> Author { get; set; }

        public TestAppDbContext(DbContextOptions<TestAppDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<District>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Phone>(b =>
            {
                b.HasKey(p => new {p.PersonId, p.Number});
            });

            modelBuilder.Entity<Person>(b =>
            {
                b.Property(x => x.LastActiveTime).ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder
                .Entity<PersonView>(p =>
                {
                    p.HasNoKey();
                    p.ToView("View_PersonView");
                });

            modelBuilder.Entity<City>(b =>
            {
                b.OwnsMany(c => c.Districts, d =>
                {
                    d.WithOwner().HasForeignKey(x => x.CityId);
                    d.HasKey(x => new {x.CityId, x.Name});
                });
            });
        }
    }
}
