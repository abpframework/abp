using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;

namespace VoloDocs.EntityFrameworkCore
{
    public class VoloDocsDbContext : AbpDbContext<VoloDocsDbContext>
    {
        public VoloDocsDbContext(DbContextOptions<VoloDocsDbContext> options) 
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
