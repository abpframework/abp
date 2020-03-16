using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Acme.BookStore.DbMigrationsForSecondDb.EntityFrameworkCore
{
    [ConnectionStringName("AbpPermissionManagement")]
    public class BookStoreSecondMigrationsDbContext : AbpDbContext<BookStoreSecondMigrationsDbContext>
    {
        public BookStoreSecondMigrationsDbContext(
            DbContextOptions<BookStoreSecondMigrationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureAuditLogging();
        }
    }
}