using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.TestApp.FourthContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.MultiTenancy;
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

    public DbSet<EntityWithCustomSoftDeleteColumn> EntityWithCustomSoftDeleteColumns { get; set; }

    public DbSet<EntityWithCustomTenantIdColumn> EntityWithCustomTenantIdColumns { get; set; }

    public DbSet<EntityWithIntSoftDelete> EntityWithIntSoftDeletes { get; set; }

    public DbSet<AppEntityWithNavigations> AppEntityWithNavigations { get; set; }
    public DbSet<AppEntityWithNavigationChildOneToMany> AppEntityWithNavigationChildOneToMany { get; set; }

    public DbSet<AppEntityWithNavigationsForeign> AppEntityWithNavigationsForeign { get; set; }

    public DbSet<AppEntityWithForeignKeyOnly> AppEntityWithForeignKeyOnly { get; set; }

    public DbSet<AppEntityWithForeignKeyOnlyChild> AppEntityWithForeignKeyOnlyChild { get; set; }

    public DbSet<AppEntityWithForeignKeyOnlyOwner> AppEntityWithForeignKeyOnlyOwner { get; set; }

    public DbSet<AppEntityWithForeignKeyOnlyEntityChild> AppEntityWithForeignKeyOnlyEntityChild { get; set; }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }

    public DbSet<TestSharedEntity> TestSharedEntity => Set<TestSharedEntity>("TestSharedEntity1");
    public DbSet<TestSharedEntity> TestSharedEntity2 => Set<TestSharedEntity>("TestSharedEntity2");

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
        // Owned and SharedTypeEntity should be configured before the base OnModelCreating call

        modelBuilder.Owned<District>();

        Action<EntityTypeBuilder<TestSharedEntity>> sharedEntityBuildAction = b =>
        {
            b.ConfigureByConvention();
            b.Property(x => x.Id);
            b.Property(x => x.TenantId);
            b.Property(x => x.IsDeleted);
            b.Property(x => x.Name);
            b.Property(x => x.Age);
            b.Property(x => x.Birthday);

            b.Property<string>("DynamicProperty");
        };
        modelBuilder.SharedTypeEntity("TestSharedEntity1", sharedEntityBuildAction);
        modelBuilder.SharedTypeEntity("TestSharedEntity2", sharedEntityBuildAction);

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
            b.ComplexProperty(x => x.ContactInformation, cb =>
            {
                cb.Property(x => x.Street).IsRequired();
                cb.ComplexProperty(x => x.Location, locationBuilder =>
                {
                    locationBuilder.Property(x => x.City).IsRequired();
                });
            });
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

        modelBuilder.Entity<EntityWithIntSoftDelete>(b =>
        {
            b.Property(x => x.IsDeleted)
                .HasColumnName(EntityWithIntSoftDelete.IsDeletedColumnName)
                .HasConversion(
                    v => v ? EntityWithIntSoftDelete.DeletedProviderValue : EntityWithIntSoftDelete.NotDeletedProviderValue,
                    i => i == EntityWithIntSoftDelete.DeletedProviderValue);
        });

        modelBuilder.Entity<AppEntityWithNavigations>(b =>
        {
            b.ConfigureByConvention();
            b.OwnsOne(v => v.AppEntityWithValueObjectAddress);
            b.HasOne(x => x.OneToOne).WithOne().HasForeignKey<AppEntityWithNavigationChildOneToOne>(x => x.Id);
            b.HasMany(x => x.OneToMany).WithOne().HasForeignKey(x => x.AppEntityWithNavigationId);
            b.HasMany(x => x.ManyToMany).WithMany(x => x.ManyToMany).UsingEntity<AppEntityWithNavigationsAndAppEntityWithNavigationChildManyToMany>();
        });

        modelBuilder.Entity<AppEntityWithNavigationsForeign>(b =>
        {
            b.ConfigureByConvention();
            b.HasMany(x => x.OneToMany).WithOne().HasForeignKey(x => x.AppEntityWithNavigationForeignId);
        });

        modelBuilder.Entity<AppEntityWithForeignKeyOnly>(b =>
        {
            b.ConfigureByConvention();
        });

        modelBuilder.Entity<AppEntityWithForeignKeyOnlyChild>(b =>
        {
            b.ConfigureByConvention();
            // No navigation property on both sides, only a foreign key.
            b.HasOne<AppEntityWithForeignKeyOnly>().WithMany().HasForeignKey(x => x.AppEntityWithForeignKeyOnlyId);
        });

        modelBuilder.Entity<AppEntityWithForeignKeyOnlyOwner>(b =>
        {
            b.ConfigureByConvention();
            b.HasMany(x => x.Children).WithOne().HasForeignKey(x => x.OwnerId);
        });

        modelBuilder.Entity<AppEntityWithForeignKeyOnlyEntityChild>(b =>
        {
            b.ConfigureByConvention();
            // The owner has a navigation, the referenced aggregate root has not.
            b.HasOne<AppEntityWithForeignKeyOnly>().WithMany().HasForeignKey(x => x.AppEntityWithForeignKeyOnlyId);
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

        modelBuilder.Entity<Blog>(b =>
        {
            b.ConfigureByConvention();
            b.HasMany(bp => bp.BlogPosts)
                .WithOne(bp => bp.Blog)
                .HasForeignKey(bp => bp.BlogId);
        });

        modelBuilder.Entity<BlogPost>(b =>
        {
            b.ConfigureByConvention();
        });

        modelBuilder.TryConfigureObjectExtensions<TestAppDbContext>();
    }

    // Renames IsDeleted / TenantId to a non-default column name and re-registers the global filter
    // afterwards. CreateFilterExpression then captures the renamed column name; before the fix it
    // would feed that string to EF.Property<T>(...), which expects a CLR property name and breaks
    // translation. Covered by SoftDelete_With_Custom_Column_Name_Tests and
    // MultiTenant_With_Custom_Column_Name_Tests.
    protected override void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
    {
        base.ConfigureBaseProperties<TEntity>(modelBuilder, mutableEntityType);

        if (typeof(EntityWithCustomSoftDeleteColumn).IsAssignableFrom(typeof(TEntity)))
        {
            modelBuilder.Entity<TEntity>()
                .Property(nameof(ISoftDelete.IsDeleted))
                .HasColumnName(EntityWithCustomSoftDeleteColumn.IsDeletedColumnName);

            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType, modelBuilder.Entity<TEntity>());
        }

        if (typeof(EntityWithCustomTenantIdColumn).IsAssignableFrom(typeof(TEntity)))
        {
            modelBuilder.Entity<TEntity>()
                .Property(nameof(IMultiTenant.TenantId))
                .HasColumnName(EntityWithCustomTenantIdColumn.TenantIdColumnName);

            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType, modelBuilder.Entity<TEntity>());
        }
    }
}
