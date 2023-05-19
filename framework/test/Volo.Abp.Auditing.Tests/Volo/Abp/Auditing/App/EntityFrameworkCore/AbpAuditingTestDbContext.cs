using Microsoft.EntityFrameworkCore;
using Volo.Abp.Auditing.App.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.Auditing.App.EntityFrameworkCore;

public class AbpAuditingTestDbContext : AbpDbContext<AbpAuditingTestDbContext>
{
    public DbSet<AppEntityWithAudited> AppEntityWithAudited { get; set; }

    public DbSet<AppEntityWithAuditedAndPropertyHasDisableAuditing> AppEntityWithAuditedAndPropertyHasDisableAuditing { get; set; }

    public DbSet<AppEntityWithDisableAuditing> AppEntityWithDisableAuditing { get; set; }

    public DbSet<AppEntityWithDisableAuditingAndPropertyHasAudited> AppEntityWithDisableAuditingAndPropertyHasAudited { get; set; }

    public DbSet<AppEntityWithPropertyHasAudited> AppEntityWithPropertyHasAudited { get; set; }

    public DbSet<AppEntityWithSelector> AppEntityWithSelector { get; set; }

    public DbSet<AppFullAuditedEntityWithAudited> AppFullAuditedEntityWithAudited { get; set; }

    public DbSet<AppEntityWithAuditedAndHasCustomAuditingProperties> AppEntityWithAuditedAndHasCustomAuditingProperties { get; set; }

    public DbSet<AppEntityWithSoftDelete> AppEntityWithSoftDelete { get; set; }

    public DbSet<AppEntityWithValueObject> AppEntityWithValueObject { get; set; }

    public AbpAuditingTestDbContext(DbContextOptions<AbpAuditingTestDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppEntityWithValueObject>(b =>
        {
            b.ConfigureByConvention();
            b.OwnsOne(v => v.AppEntityWithValueObjectAddress);
        });
    }
}
