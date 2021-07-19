using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(IFourthDbContext))]
    public class TestAppDbContext : AbpDbContext<TestAppDbContext>, IThirdDbContext, IFourthDbContext
    {
        private DbSet<FourthDbContextDummyEntity> _dummyEntities;
        private DbSet<FourthDbContextDummyEntity> _dummyEntities1;
        public DbSet<Person> People { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<PersonView> PersonView { get; set; }

        public DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }

        public DbSet<EntityWithIntPk> EntityWithIntPks { get; set; }

        public DbSet<Author> Author { get; set; }

        public DbSet<FourthDbContextDummyEntity> FourthDummyEntities { get; set; }

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

                b.ApplyObjectExtensionMappings();
            });

            modelBuilder.Entity<Person>(b =>
            {
                b.Property(x => x.LastActiveTime).ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.Now);
            });

            modelBuilder
                .Entity<PersonView>(p =>
                {
                    p.HasNoKey();
                    p.ToView("View_PersonView");

                    p.ApplyObjectExtensionMappings();
                });

            modelBuilder.Entity<City>(b =>
            {
                b.OwnsMany(c => c.Districts, d =>
                {
                    d.WithOwner().HasForeignKey(x => x.CityId);
                    d.HasKey(x => new {x.CityId, x.Name});
                });

                b.ApplyObjectExtensionMappings();
            });

            modelBuilder.TryConfigureObjectExtensions<TestAppDbContext>();
        }
    }
}
