using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.OpenIddict
{
    public static class AbpOpenIddictBuilderExtensions
    {
        public static OpenIddictBuilder AddAbpOpenIddict(this OpenIddictBuilder builder,
            AbpOpenIddictBuilderOptions options = null)
        {
            if (options == null)
            {
                options = new AbpOpenIddictBuilderOptions();
            }

            if (options.UpdateAbpClaimTypes)
            {
                AbpClaimTypes.UserId = Claims.Subject;
                AbpClaimTypes.UserName = Claims.Name;
                AbpClaimTypes.Role = Claims.Role;
                AbpClaimTypes.Email = Claims.Email;
            }

            if (options.UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap)
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.UserId] = AbpClaimTypes.UserId;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.UserName] = AbpClaimTypes.UserName;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.Role] = AbpClaimTypes.Role;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[AbpClaimTypes.Email] = AbpClaimTypes.Email;
            }

            return builder;
        }
    }
}
