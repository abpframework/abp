using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(MyProjectNameDomainModule),
    typeof(MyProjectNameTestBaseModule)
)]
public class MyProjectNameDomainTestModule : AbpModule
{

}
