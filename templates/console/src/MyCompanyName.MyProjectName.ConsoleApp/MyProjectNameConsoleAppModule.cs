using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.ConsoleApp
{

    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class MyProjectNameConsoleAppModule : AbpModule
    {

    }
}
