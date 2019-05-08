using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation
{
    [DependsOn(
        typeof(AbpValidationModule)
        )]
    public class AbpFluentValidationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpFluentValidationConventionalRegistrar());
        }
    }
}
