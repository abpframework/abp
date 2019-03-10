using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Abp.FeatureManagement.MongoDB
{
    [ConnectionStringName("FeatureManagement")]
    public class FeatureManagementMongoDbContext : AbpMongoDbContext, IFeatureManagementMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = FeatureManagementConsts.DefaultDbTablePrefix;

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

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