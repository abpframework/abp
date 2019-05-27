using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Users
{
    //TODO: Consider to (somehow) move this to the framework to the same assemblily of ICurrentUser!

    [DependsOn(
        typeof(MultiTenancyModule),
        typeof(EventBusModule)
        )]
    public class UsersAbstractionModule : AbpModule
    {

    }
}
