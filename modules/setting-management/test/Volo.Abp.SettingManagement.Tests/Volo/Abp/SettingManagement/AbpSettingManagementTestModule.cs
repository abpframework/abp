using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Users;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingManagementEntityFrameworkCoreTestModule), 
        typeof(AbpUsersAbstractionModule))]
    public class AbpSettingManagementTestModule : AbpModule //TODO: Rename to Volo.Abp.SettingManagement.Domain.Tests..?
    {
        
    }
}
