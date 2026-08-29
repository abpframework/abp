using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.FluentValidation;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpHttpFluentValidationModule)
    )]
public class AbpHttpFluentValidationTestModule : AbpModule
{
}
