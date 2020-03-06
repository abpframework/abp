using Volo.Abp.Modularity;

namespace Volo.Abp.Http.Client.IdentityModel.Web.Tests
{
    [DependsOn(typeof(AbpHttpClientIdentityModelWebModule))]
    public class AbpHttpClientIdentityModelWebTestModule : AbpModule
    {

    }
}
