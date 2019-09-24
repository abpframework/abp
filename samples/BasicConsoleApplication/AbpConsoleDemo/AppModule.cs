using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpConsoleDemo
{
    [DependsOn(
        typeof(AbpAutofacModule)
        )]
    public class AppModule : AbpModule
    {

    }
}
