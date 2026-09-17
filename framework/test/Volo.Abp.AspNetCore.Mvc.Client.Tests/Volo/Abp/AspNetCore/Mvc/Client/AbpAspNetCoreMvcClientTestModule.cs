using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client;

[DependsOn(
    typeof(AbpAspNetCoreMvcClientModule),
    typeof(AbpAutofacModule)
)]
public class AbpAspNetCoreMvcClientTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();
    }
}
