using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Blogging.EntityFrameworkCore;
using Volo.Docs.EntityFrameworkCore;
using Volo.Utils.SolutionTemplating;

namespace Volo.AbpWebSite.EntityFrameworkCore
{
    public class AbpWebSiteDbContext : AbpDbContext<AbpWebSiteDbContext>
    {
        public DbSet<DownloadInfo> Downloads { get; set; }

        public AbpWebSiteDbContext(DbContextOptions<AbpWebSiteDbContext> options) 
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
            modelBuilder.ConfigureBlogging();
        }
    }
}
