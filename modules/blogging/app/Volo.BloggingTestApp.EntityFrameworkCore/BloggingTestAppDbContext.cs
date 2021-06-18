using Microsoft.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;

namespace Volo.BloggingTestApp.EntityFrameworkCore
{
    public class BloggingTestAppDbContext : AbpDbContext<BloggingTestAppDbContext>
    {
        public BloggingTestAppDbContext(DbContextOptions<BloggingTestAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureSettingManagement();
            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigureBlogging();
            modelBuilder.ConfigureBlobStoring();
        }
    }
}
