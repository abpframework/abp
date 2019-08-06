using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameDomainSharedModule)
        )]
    public class MyProjectNameDomainModule : AbpModule
    {

    }
}
