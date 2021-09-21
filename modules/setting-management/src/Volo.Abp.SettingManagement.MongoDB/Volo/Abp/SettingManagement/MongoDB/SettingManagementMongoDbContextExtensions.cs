using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    public static class SettingManagementMongoDbContextExtensions
    {
        public static void ConfigureSettingManagement(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Setting>(b =>
            {
                b.CollectionName = AbpSettingManagementDbProperties.DbTablePrefix + "Settings";
            });
        }
    }
}