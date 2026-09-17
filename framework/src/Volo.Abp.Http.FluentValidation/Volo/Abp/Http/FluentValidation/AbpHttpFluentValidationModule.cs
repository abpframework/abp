using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.FluentValidation;

[DependsOn(
    typeof(AbpHttpModule),
    typeof(AbpFluentValidationModule)
    )]
public class AbpHttpFluentValidationModule : AbpModule
{
}
