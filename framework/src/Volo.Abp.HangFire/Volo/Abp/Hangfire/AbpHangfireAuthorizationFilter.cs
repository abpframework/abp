using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Hangfire;

public class AbpHangfireAuthorizationFilter : IDashboardAsyncAuthorizationFilter
{
    private readonly bool _enableTenant;
    private readonly AuthorizationPolicyBuilder _policyBuilder;

    public virtual AuthorizationPolicyBuilder PolicyBuilder => _policyBuilder;

    public AbpHangfireAuthorizationFilter(bool enableTenant = false, string? requiredPermissionName = null, params string[]? requiredRoleNames)
    {
        _enableTenant = enableTenant;
        _policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
        if (!requiredPermissionName.IsNullOrWhiteSpace())
        {
            _policyBuilder.Requirements.Add(new PermissionRequirement(requiredPermissionName));
        }

        if (!requiredRoleNames.IsNullOrEmpty())
        {
            foreach (var roleName in requiredRoleNames!)
            {
                _policyBuilder.RequireRole(roleName);
            }
        }
    }

    public virtual async Task<bool> AuthorizeAsync(DashboardContext context)
    {
        var currentTenant = context.GetHttpContext().RequestServices.GetRequiredService<ICurrentTenant>();
        if (currentTenant.IsAvailable && !_enableTenant)
        {
            return false;
        }

        var authorizationService = context.GetHttpContext().RequestServices.GetRequiredService<IAuthorizationService>();
        var authorizationPolicy = _policyBuilder.Build();
        return (await authorizationService.AuthorizeAsync(context.GetHttpContext().User, authorizationPolicy)).Succeeded;
    }
}
