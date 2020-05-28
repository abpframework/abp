using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public class BlobStoringDatabaseMongoDbContext : AbpMongoDbContext, IBlobStoringDatabaseMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureDatabase();
        }
    }
}