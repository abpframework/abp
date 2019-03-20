using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompanyName.MyProjectName.Users;
using Volo.Abp;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public static class MyProjectNameDbContextModelCreatingExtensions
    {
        public static void ConfigureMyProjectName(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            var tablePrefix = MyProjectNameConsts.DefaultDbTablePrefix;
            var schema = MyProjectNameConsts.DefaultDbSchema;

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(tablePrefix + "YourEntities", schema);

            //    //...
            //});
        }

        public static void ConfigureCustomUserProperties<TUser>(this EntityTypeBuilder<TUser> b)
            where TUser: class, IUser
        {
            //b.Property(nameof(AppUser.MyProperty))...
        }
    }
}