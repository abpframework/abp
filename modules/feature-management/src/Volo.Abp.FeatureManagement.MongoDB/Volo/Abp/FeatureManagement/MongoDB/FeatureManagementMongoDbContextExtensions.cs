using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    public static class FeatureManagementMongoDbContextExtensions
    {
        public static void ConfigureFeatureManagement(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<FeatureValue>(b =>
            {
                b.CollectionName = FeatureManagementDbProperties.DbTablePrefix + "FeatureValues";
            });
        }
    }
}