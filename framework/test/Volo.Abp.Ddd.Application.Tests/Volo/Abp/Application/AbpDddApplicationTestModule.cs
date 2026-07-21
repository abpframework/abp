using Volo.Abp.Autofac;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Volo.Abp.Application;

[DependsOn(typeof(AbpAutofacModule))]
[DependsOn(typeof(AbpDddApplicationModule))]
[DependsOn(typeof(AbpExceptionHandlingModule))]
public class AbpDddApplicationTestModule : AbpModule
{
}
