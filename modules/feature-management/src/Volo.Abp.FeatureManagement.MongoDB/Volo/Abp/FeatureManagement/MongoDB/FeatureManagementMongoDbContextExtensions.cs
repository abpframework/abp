using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB;

public static class FeatureManagementMongoDbContextExtensions
{
    public static void ConfigureFeatureManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<FeatureGroupDefinitionRecord>(b =>
        {
            b.CollectionName = AbpFeatureManagementDbProperties.DbTablePrefix + "FeatureGroups";
        });

        builder.Entity<FeatureDefinitionRecord>(b =>
        {
            b.CollectionName = AbpFeatureManagementDbProperties.DbTablePrefix + "Features";
        });

        builder.Entity<FeatureValue>(b =>
        {
            b.CollectionName = AbpFeatureManagementDbProperties.DbTablePrefix + "FeatureValues";
        });
    }
}
