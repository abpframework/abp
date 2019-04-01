using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace Volo.DocsTestApp.EntityFrameworkCore
{
    public class DocsTestAppDbContext : AbpDbContext<DocsTestAppDbContext>
    {
        public DocsTestAppDbContext(DbContextOptions<DocsTestAppDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureSettingManagement();
            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigureDocs();
        }
    }
}
