using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public class MyProjectNameDbContext : AbpDbContext<MyProjectNameDbContext>
    {
        public MyProjectNameDbContext(DbContextOptions<MyProjectNameDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureSettingManagement();

            //builder.Entity<MyEntity>(b =>
            //{
            //    b.ToTable(MyProjectNameConsts.DbTablePrefix + "PermissionGrants", MyProjectNameConsts.DbSchema);

            //    ...
            //});
        }
    }
}
