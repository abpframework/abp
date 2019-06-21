using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCompanyName.MyProjectName.Users;
using Volo.Abp;
using Volo.Abp.Reflection;
using Volo.Abp.Users;

namespace MyCompanyName.MyProjectName.EntityFrameworkCore
{
    public static class MyProjectNameDbContextModelCreatingExtensions
    {
        public static void ConfigureMyProjectName(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(MyProjectNameConsts.DbTablePrefix + "YourEntities", MyProjectNameConsts.DbSchema);

            //    //...
            //});
        }

        public static void ConfigureCustomUserProperties<TUser>(this EntityTypeBuilder<TUser> b)
            where TUser: class, IUser
        {
            var type = typeof(AppUser);
            var properties = new List<PropertyInfo>();

            properties.AddRange(type.GetProperties()
                .Where(p => !ReflectionHelper.GetPublicProperties(type.GetInterface("IUser")).Select(ip => ip.Name).Contains(p.Name))
                .Where(p => !type.BaseType.GetProperties().Select(bp => bp.Name).Contains(p.Name)));

            properties.ForEach(p =>
            {
                var mi = b.GetType().GetMethods().First(method => method.Name == "Property" && method.IsGenericMethod && method.GetParameters().Length == 1 && method.GetParameters().Single().ParameterType == typeof(string)).MakeGenericMethod(p.PropertyType);
                mi.Invoke(b, new object[] { p.Name });
            });
        }
    }
}