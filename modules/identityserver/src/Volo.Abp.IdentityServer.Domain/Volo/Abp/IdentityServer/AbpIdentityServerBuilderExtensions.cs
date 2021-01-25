using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.AspNetIdentity;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer
{
    public static class AbpIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddAbpIdentityServer(
            this IIdentityServerBuilder builder,
            AbpIdentityServerBuilderOptions options = null)
        {
            if (options == null)
            {
                options = new AbpIdentityServerBuilderOptions();
            }

            //TODO: AspNet Identity integration lines. Can be extracted to a extension method
            if (options.IntegrateToAspNetIdentity)
            {
                builder.AddAspNetIdentity<IdentityUser>();
                builder.AddProfileService<AbpProfileService>();
                builder.AddResourceOwnerValidator<AbpResourceOwnerPasswordValidator>();

                builder.Services.Remove(builder.Services.LastOrDefault(x => x.ServiceType == typeof(IUserClaimsPrincipalFactory<IdentityUser>)));
                builder.Services.AddTransient<IUserClaimsPrincipalFactory<IdentityUser>, AbpUserClaimsFactory<IdentityUser>>();
                builder.Services.AddTransient<IObjectAccessor<IUserClaimsPrincipalFactory<IdentityUser>>, ObjectAccessor<AbpUserClaimsPrincipalFactory>>();
            }

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
