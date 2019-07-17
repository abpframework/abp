using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    /* Domain tests are configured to use the EF Core provider.
     * You can switch to MongoDB, however your domain tests should be
     * database independent anyway.
     */
    [DependsOn(
        typeof(MyProjectNameEntityFrameworkCoreTestModule)
        )]
    public class MyProjectNameDomainTestModule : AbpModule
    {
        
    }
}
