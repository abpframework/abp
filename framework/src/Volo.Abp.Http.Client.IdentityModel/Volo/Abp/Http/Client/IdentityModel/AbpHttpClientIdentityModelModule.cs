using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(AbpIdentityModelModule)
    )]
public class AbpHttpClientIdentityModelModule : AbpModule
{

}
