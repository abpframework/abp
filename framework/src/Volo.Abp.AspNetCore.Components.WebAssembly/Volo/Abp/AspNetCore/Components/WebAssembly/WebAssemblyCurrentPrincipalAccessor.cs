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

        public WebAssemblyCurrentPrincipalAccessor(ICachedApplicationConfigurationClient configurationClient)
        {
            ConfigurationClient = configurationClient;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            //TODO: Should be optimized! Or should be replaced?

            var configuration = ConfigurationClient.Get();

            var claims = new List<Claim>();

            claims.Add(new Claim(AbpClaimTypes.UserName,configuration.CurrentUser.UserName));
            claims.Add(new Claim(AbpClaimTypes.Email,configuration.CurrentUser.Email));
            claims.Add(new Claim(AbpClaimTypes.UserId,configuration.CurrentUser.Id.ToString()));
            claims.Add(new Claim(AbpClaimTypes.TenantId,configuration.CurrentUser.TenantId.ToString()));

            foreach (var role in configuration.CurrentUser.Roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role));
            }

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
