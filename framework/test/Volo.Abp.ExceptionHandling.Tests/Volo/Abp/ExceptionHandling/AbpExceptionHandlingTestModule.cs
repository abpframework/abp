using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.ExceptionHandling;

[DependsOn(
    typeof(AbpExceptionHandlingModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpExceptionHandlingTestModule : AbpModule
{

}
