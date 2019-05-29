using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameApplicationContractsModule),
        typeof(AspNetCoreMvcModule))]
    public class MyProjectNameHttpApiModule : AbpModule
    {
        
    }
}
