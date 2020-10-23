using IdentityModel;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Http.Client.IdentityModel.WebAssembly
{
    [DependsOn(
        typeof(AbpHttpClientIdentityModelModule)
    )]
    public class AbpHttpClientIdentityModelWebAssemblyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AbpClaimTypes.UserName = JwtClaimTypes.PreferredUserName;
            //AbpClaimTypes.Name = ...; //TODO
            //AbpClaimTypes.SurName = ...; //TODO
            AbpClaimTypes.UserId = JwtClaimTypes.Subject;
            AbpClaimTypes.Role = JwtClaimTypes.Role;
            AbpClaimTypes.Email = JwtClaimTypes.Email;
        }
    }
}
