using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.EntityFrameworkCore
{
    public class TestMigrationsDbContext : AbpDbContext<TestMigrationsDbContext>
    {
        public DbSet<Person> People { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }

        public DbSet<BookInSecondDbContext> Books { get; set; }

        public DbSet<EntityWithIntPk> EntityWithIntPks { get; set; }

        public DbSet<Author> Author { get; set; }

        public TestMigrationsDbContext(DbContextOptions<TestMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<District>();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Phone>(b =>
            {
                b.HasKey(p => new { p.PersonId, p.Number });
            });


            modelBuilder.Entity<Person>(b =>
            {
                b.Property(x => x.LastActiveTime).ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<City>(b =>
            {
                b.OwnsMany(c => c.Districts, d =>
                {
                    d.WithOwner().HasForeignKey(x => x.CityId);
                    d.HasKey(x => new { x.CityId, x.Name });
                });
            });
        }
    }
}
