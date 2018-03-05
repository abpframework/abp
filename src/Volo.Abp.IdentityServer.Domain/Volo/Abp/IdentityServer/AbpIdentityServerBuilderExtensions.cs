using System;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer
{
    public static class AbpIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddAbpIdentityServer(
            this IIdentityServerBuilder builder, 
            Action<AbpIdentityServerOptions> optionsAction = null)
        {
            var options = new AbpIdentityServerOptions();
            optionsAction?.Invoke(options);

            //TODO: AspNet Identity integration lines. Can be extracted to a extension method
            builder.AddAspNetIdentity<IdentityUser>();
            builder.AddProfileService<AbpProfileService>();
            builder.AddResourceOwnerValidator<AbpResourceOwnerPasswordValidator>();

            builder.Services.Replace(ServiceDescriptor.Transient<IClaimsService, AbpClaimsService>());

            if (options.UpdateAbpClaimTypes)
            {
                AbpClaimTypes.UserId = JwtClaimTypes.Subject;
                AbpClaimTypes.UserName = JwtClaimTypes.Name;
                AbpClaimTypes.Role = JwtClaimTypes.Role;
                AbpClaimTypes.Email = JwtClaimTypes.Email;
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