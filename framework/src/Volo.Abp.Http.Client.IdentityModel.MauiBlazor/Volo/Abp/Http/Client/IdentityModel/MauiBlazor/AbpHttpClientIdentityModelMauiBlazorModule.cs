using IdentityModel;
using Volo.Abp.AspNetCore.Components.MauiBlazor;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

[DependsOn(
    typeof(AbpHttpClientIdentityModelModule),
    typeof(AbpAspNetCoreComponentsMauiBlazorModule)
)]
public class AbpHttpClientIdentityModelMauiBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AbpClaimTypes.UserName = JwtClaimTypes.PreferredUserName;
        AbpClaimTypes.Name = JwtClaimTypes.GivenName;
        AbpClaimTypes.SurName = JwtClaimTypes.FamilyName;
        AbpClaimTypes.UserId = JwtClaimTypes.Subject;
        AbpClaimTypes.Role = JwtClaimTypes.Role;
        AbpClaimTypes.Email = JwtClaimTypes.Email;
    }
}
