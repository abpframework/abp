using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(MyProjectNameDomainSharedModule)
)]
public class MyProjectNameDomainModule : AbpModule
{

}
