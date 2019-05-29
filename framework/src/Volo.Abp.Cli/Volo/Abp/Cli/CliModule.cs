using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(CliCoreModule),
        typeof(AutofacModule)
    )]
    public class CliModule : AbpModule
    {

    }
}