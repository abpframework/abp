using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    [ConnectionStringName("AbpFeatureManagement")]
    public class FeatureManagementMongoDbContext : AbpMongoDbContext, IFeatureManagementMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = FeatureManagementConsts.DefaultDbTablePrefix;

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureFeatureManagement(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}