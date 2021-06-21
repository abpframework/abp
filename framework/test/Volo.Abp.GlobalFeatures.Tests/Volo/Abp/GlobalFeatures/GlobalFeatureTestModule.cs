using Volo.Abp.Application;
using Volo.Abp.Autofac;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Volo.Abp.GlobalFeatures
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpGlobalFeaturesModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpExceptionHandlingModule))]
    public class GlobalFeatureTestModule : AbpModule
    {

    }
}
