using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(MyProjectNameApplicationModule),
    typeof(MyProjectNameDomainTestModule)
    )]
public class MyProjectNameApplicationTestModule : AbpModule
{

}
