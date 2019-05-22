using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationModule),
        typeof(MyProjectNameEntityFrameworkCoreTestModule)
        )]
    public class MyProjectNameApplicationTestModule : AbpModule
    {

    }
}