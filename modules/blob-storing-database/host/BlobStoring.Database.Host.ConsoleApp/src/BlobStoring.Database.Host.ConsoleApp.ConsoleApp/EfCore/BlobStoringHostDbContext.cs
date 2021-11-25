using Microsoft.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp.EfCore;

public class BlobStoringHostDbContext : AbpDbContext<BlobStoringHostDbContext>
{
    public BlobStoringHostDbContext(DbContextOptions<BlobStoringHostDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureBlobStoring();
    }
}
