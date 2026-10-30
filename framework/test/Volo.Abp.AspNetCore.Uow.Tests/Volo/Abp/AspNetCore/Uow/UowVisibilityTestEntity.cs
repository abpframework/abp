using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.AspNetCore.Uow;

public class UowVisibilityTestEntity : AggregateRoot<Guid>
{
    public string Name { get; set; }

    protected UowVisibilityTestEntity()
    {
    }

    public UowVisibilityTestEntity(Guid id, string name)
        : base(id)
    {
        Name = name;
    }
}

[ConnectionStringName("Default")]
public class UowVisibilityTestDbContext : AbpDbContext<UowVisibilityTestDbContext>
{
    public DbSet<UowVisibilityTestEntity> UowVisibilityTestEntities { get; set; }

    public UowVisibilityTestDbContext(DbContextOptions<UowVisibilityTestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UowVisibilityTestEntity>(b =>
        {
            b.ToTable("UowVisibilityTestEntities");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired();
        });
    }
}
