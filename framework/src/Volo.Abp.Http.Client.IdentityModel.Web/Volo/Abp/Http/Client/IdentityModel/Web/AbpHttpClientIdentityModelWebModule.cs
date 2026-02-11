using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel.Web;

[DependsOn(
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AbpHttpClientIdentityModelWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();
    }
}
