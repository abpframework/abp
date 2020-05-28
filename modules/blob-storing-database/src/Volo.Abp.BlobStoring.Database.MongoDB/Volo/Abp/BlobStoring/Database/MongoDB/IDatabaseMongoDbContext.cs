using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [ConnectionStringName(DatabaseDbProperties.ConnectionStringName)]
    public interface IDatabaseMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
