using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    [ConnectionStringName(FeatureManagementDbProperties.ConnectionStringName)]
    public class FeatureManagementMongoDbContext : AbpMongoDbContext, IFeatureManagementMongoDbContext
    {
        public IMongoCollection<FeatureValue> FeatureValues => Collection<FeatureValue>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureFeatureManagement();
        }
    }
}