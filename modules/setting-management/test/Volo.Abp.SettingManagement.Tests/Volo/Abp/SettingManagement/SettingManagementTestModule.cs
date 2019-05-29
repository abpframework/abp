using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Users;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(SettingManagementEntityFrameworkCoreTestModule), 
        typeof(UsersAbstractionModule))]
    public class SettingManagementTestModule : AbpModule //TODO: Rename to Volo.Abp.SettingManagement.Domain.Tests..?
    {
        
    }
}
