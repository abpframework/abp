using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class WebAssemblyRemoteCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ITransientDependency
{
    protected ApplicationConfigurationCache ApplicationConfigurationCache { get; }

    public WebAssemblyRemoteCurrentPrincipalAccessor(ApplicationConfigurationCache applicationConfigurationCache)
    {
        ApplicationConfigurationCache = applicationConfigurationCache;
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        var applicationConfiguration = ApplicationConfigurationCache.Get();
        if (applicationConfiguration == null || !applicationConfiguration.CurrentUser.IsAuthenticated)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var claims = new List<Claim>()
        {
            new Claim(AbpClaimTypes.UserId, applicationConfiguration.CurrentUser.Id.ToString()!),
        };

        if (applicationConfiguration.CurrentUser.TenantId != null)
        {
            claims.Add(new Claim(AbpClaimTypes.TenantId, applicationConfiguration.CurrentUser.TenantId.ToString()!));
        }
        if (applicationConfiguration.CurrentUser.ImpersonatorUserId != null)
        {
            claims.Add(new Claim(AbpClaimTypes.ImpersonatorUserId, applicationConfiguration.CurrentUser.ImpersonatorUserId.ToString()!));
        }
        if (applicationConfiguration.CurrentUser.ImpersonatorTenantId != null)
        {
            claims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantId, applicationConfiguration.CurrentUser.ImpersonatorTenantId.ToString()!));
        }
        if (applicationConfiguration.CurrentUser.ImpersonatorUserName != null)
        {
            claims.Add(new Claim(AbpClaimTypes.ImpersonatorUserName, applicationConfiguration.CurrentUser.ImpersonatorUserName));
        }
        if (applicationConfiguration.CurrentUser.ImpersonatorTenantName != null)
        {
            claims.Add(new Claim(AbpClaimTypes.ImpersonatorTenantName, applicationConfiguration.CurrentUser.ImpersonatorTenantName));
        }
        if (applicationConfiguration.CurrentUser.UserName != null)
        {
            claims.Add(new Claim(AbpClaimTypes.UserName, applicationConfiguration.CurrentUser.UserName));
        }
        if (applicationConfiguration.CurrentUser.Name != null)
        {
            claims.Add(new Claim(AbpClaimTypes.Name, applicationConfiguration.CurrentUser.Name));
        }
        if (applicationConfiguration.CurrentUser.SurName != null)
        {
            claims.Add(new Claim(AbpClaimTypes.SurName, applicationConfiguration.CurrentUser.SurName));
        }
        if (applicationConfiguration.CurrentUser.Email != null)
        {
            claims.Add(new Claim(AbpClaimTypes.Email, applicationConfiguration.CurrentUser.Email));
        }
        if (applicationConfiguration.CurrentUser.EmailVerified)
        {
            claims.Add(new Claim(AbpClaimTypes.EmailVerified, applicationConfiguration.CurrentUser.EmailVerified.ToString()));
        }
        if (applicationConfiguration.CurrentUser.PhoneNumber != null)
        {
            claims.Add(new Claim(AbpClaimTypes.PhoneNumber, applicationConfiguration.CurrentUser.PhoneNumber));
        }
        if (applicationConfiguration.CurrentUser.PhoneNumberVerified)
        {
            claims.Add(new Claim(AbpClaimTypes.PhoneNumberVerified, applicationConfiguration.CurrentUser.PhoneNumberVerified.ToString()));
        }
        if (applicationConfiguration.CurrentUser.SessionId != null)
        {
            claims.Add(new Claim(AbpClaimTypes.SessionId, applicationConfiguration.CurrentUser.SessionId));
        }

        if (!applicationConfiguration.CurrentUser.Roles.IsNullOrEmpty())
        {
            foreach (var role in applicationConfiguration.CurrentUser.Roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role));
            }
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(WebAssemblyRemoteCurrentPrincipalAccessor)));
    }
}
