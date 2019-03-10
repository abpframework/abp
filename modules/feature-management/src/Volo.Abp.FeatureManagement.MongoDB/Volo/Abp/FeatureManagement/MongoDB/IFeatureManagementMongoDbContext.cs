using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    [ConnectionStringName("AbpFeatureManagement")]
    public interface IFeatureManagementMongoDbContext : IAbpMongoDbContext
    {

    }
}
