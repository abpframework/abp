using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpHttpModule)
    )]
public class AbpFluentValidationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConventionalRegistrar(new AbpFluentValidationConventionalRegistrar());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
        context.Services.AddTransient<IPropertyApiDescriptionModelContributor, FluentValidationApiDescriptionModelContributor>();
    }
}
