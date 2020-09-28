using System;
using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
    {
        protected ICachedApplicationConfigurationClient ConfigurationClient { get; }

        public WebAssemblyCurrentPrincipalAccessor(
            ICachedApplicationConfigurationClient configurationClient)
        {
            ConfigurationClient = configurationClient;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            var configuration = ConfigurationClient.Get();

            var claims = new List<Claim>();

            if (configuration.CurrentUser.Id != null)
            {
                claims.Add(new Claim(AbpClaimTypes.UserId,configuration.CurrentUser.Id.ToString()));
            }

            if (!configuration.CurrentUser.UserName.IsNullOrWhiteSpace())
            {
                claims.Add(new Claim(AbpClaimTypes.UserName,configuration.CurrentUser.UserName));
            }

            if (!configuration.CurrentUser.Email.IsNullOrWhiteSpace())
            {
                claims.Add(new Claim(AbpClaimTypes.Email,configuration.CurrentUser.Email));
            }

            if (configuration.CurrentUser.EmailVerified)
            {
                claims.Add(new Claim(AbpClaimTypes.EmailVerified,"true"));
            }

            if (!configuration.CurrentUser.Name.IsNullOrWhiteSpace())
            {
                claims.Add(new Claim(AbpClaimTypes.Name,configuration.CurrentUser.Name));
            }

            if (!configuration.CurrentUser.PhoneNumber.IsNullOrEmpty())
            {
                claims.Add(new Claim(AbpClaimTypes.PhoneNumber,configuration.CurrentUser.PhoneNumber));
            }

            if (configuration.CurrentUser.PhoneNumberVerified)
            {
                claims.Add(new Claim(AbpClaimTypes.PhoneNumberVerified,"true"));
            }

            if (!configuration.CurrentUser.SurName.IsNullOrWhiteSpace())
            {
                claims.Add(new Claim(AbpClaimTypes.SurName,configuration.CurrentUser.SurName));
            }

            if (configuration.CurrentUser.TenantId != null)
            {
                claims.Add(new Claim(AbpClaimTypes.TenantId,configuration.CurrentUser.TenantId.ToString()));
            }
            else if (configuration.CurrentTenant.Id != null)
            {
                claims.Add(new Claim(AbpClaimTypes.TenantId,configuration.CurrentTenant.Id.ToString()));
            }

            foreach (var role in configuration.CurrentUser.Roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role));
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
