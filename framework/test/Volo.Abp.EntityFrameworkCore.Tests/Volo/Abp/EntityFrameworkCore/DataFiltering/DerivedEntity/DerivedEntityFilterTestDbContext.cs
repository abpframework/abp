using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

public class DerivedEntityFilterTestDbContext : DerivedEntityFilterTestDbContextBase<DerivedEntityFilterTestDbContext>
{
    public DerivedEntityFilterTestDbContext(DbContextOptions<DerivedEntityFilterTestDbContext> options) : base(options)
    {
    }
}

public abstract class DerivedEntityFilterTestDbContextBase<TDbContext> : AbpDbContext<TDbContext>
    where TDbContext : DbContext
{
    public DbSet<Notice> Notices { get; set; }

    public DbSet<TenantNotice> TenantNotices { get; set; }

    public DbSet<Announcement> Announcements { get; set; }

    public DbSet<TenantAnnouncement> TenantAnnouncements { get; set; }

    protected virtual bool UseTpt => false;

    protected DerivedEntityFilterTestDbContextBase(DbContextOptions<TDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (UseTpt)
        {
            modelBuilder.Entity<Notice>().UseTptMappingStrategy();
            modelBuilder.Entity<TenantNotice>().ToTable("TenantNotices");
            modelBuilder.Entity<Announcement>().UseTptMappingStrategy();
            modelBuilder.Entity<TenantAnnouncement>().ToTable("TenantAnnouncements");
        }

        modelBuilder.Entity<Notice>(b =>
        {
            b.ToTable("Notices");
            b.ConfigureByConvention();
        });

        modelBuilder.Entity<TenantNotice>(b =>
        {
            b.ConfigureByConvention();
        });

        modelBuilder.Entity<Announcement>(b =>
        {
            b.ToTable("Announcements");
            b.ConfigureByConvention();
        });

        modelBuilder.Entity<TenantAnnouncement>(b =>
        {
            b.ConfigureByConvention();
        });
    }
}
