using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.Testing;

namespace Volo.Abp.TestApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IFourthDbContext))]
public class TestAppDbContext : AbpDbContext<TestAppDbContext>, IThirdDbContext, IFourthDbContext
{
    public DbSet<Person> People { get; set; }

    public DbSet<City> Cities { get; set; }

    public DbSet<PersonView> PersonView { get; set; }

    public DbSet<ThirdDbContextDummyEntity> DummyEntities { get; set; }

    public DbSet<EntityWithIntPk> EntityWithIntPks { get; set; }

    public DbSet<Author> Author { get; set; }

    public DbSet<FourthDbContextDummyEntity> FourthDummyEntities { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<AppEntityWithNavigations> AppEntityWithNavigations { get; set; }

    public DbSet<AppEntityWithNavigationsForeign> AppEntityWithNavigationsForeign { get; set; }

    public TestAppDbContext(DbContextOptions<TestAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, UnitTestModelCacheKeyFactory>();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Owned<District>();

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Phone>(b =>
        {
            b.HasKey(p => new { p.PersonId, p.Number });

            b.ApplyObjectExtensionMappings();
        });

        modelBuilder.Entity<Person>(b =>
        {
            b.Property(x => x.LastActiveTime).ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.Now);
            b.Property(x => x.HasDefaultValue).HasDefaultValue(DateTime.Now);
            b.Property(x => x.TenantId).HasColumnName("Tenant_Id");
            b.Property(x => x.IsDeleted).HasColumnName("Is_Deleted");
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
                d.HasKey(x => new { x.CityId, x.Name });
            });

            b.ApplyObjectExtensionMappings();
        });

        modelBuilder.Entity<Product>();

        modelBuilder.Entity<Category>(b =>
        {
            b.HasAbpQueryFilter(e => e.Name.StartsWith("abp"));
        });

        modelBuilder.Entity<AppEntityWithNavigations>(b =>
        {
            b.ConfigureByConvention();
            b.OwnsOne(v => v.AppEntityWithValueObjectAddress);
            b.HasOne(x => x.OneToOne).WithOne().HasForeignKey<AppEntityWithNavigationChildOneToOne>(x => x.Id);
            b.HasMany(x => x.OneToMany).WithOne().HasForeignKey(x => x.AppEntityWithNavigationId);
            b.HasMany(x => x.ManyToMany).WithMany(x => x.ManyToMany).UsingEntity<AppEntityWithNavigationsAndAppEntityWithNavigationChildManyToMany>();
            b.HasOne<AppEntityWithNavigationsForeign>().WithMany().HasForeignKey(x => x.AppEntityWithNavigationForeignId).IsRequired(false);
        });

        modelBuilder.Entity<AppEntityWithNavigationChildOneToOne>(b =>
        {
            b.ConfigureByConvention();
            b.HasOne(x => x.OneToOne).WithOne().HasForeignKey<AppEntityWithNavigationChildOneToOneAndOneToOne>(x => x.Id);
        });

        modelBuilder.Entity<AppEntityWithNavigationChildOneToMany>(b =>
        {
            b.ConfigureByConvention();
            b.HasMany(x => x.OneToMany).WithOne().HasForeignKey(x => x.AppEntityWithNavigationChildOneToManyId);
        });

        modelBuilder.Entity<AppEntityWithNavigationsForeign>(b =>
        {
            b.ConfigureByConvention();
        });

        modelBuilder.TryConfigureObjectExtensions<TestAppDbContext>();
    }
}
