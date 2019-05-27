using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Users.MongoDB
{
    [DependsOn(
        typeof(UsersDomainModule),
        typeof(MongoDbModule)
        )]
    public class UsersMongoDbModule : AbpModule
    {
        
    }
}
