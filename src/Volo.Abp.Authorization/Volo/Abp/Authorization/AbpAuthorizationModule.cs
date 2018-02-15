using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Authorization
{
    public class AbpAuthorizationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AllowedPermission", policy =>
                {
                    policy.Requirements.Add(new RequirePermissionRequirement
                    {
                        PermissionName = "AllowedPermission"
                    });
                });

                options.AddPolicy("NotAllowedPermission", policy =>
                {
                    policy.Requirements.Add(new RequirePermissionRequirement { PermissionName = "NotAllowedPermission" });
                });
            });

            services.AddSingleton<IAuthorizationHandler, RequirePermissionHandler>();

            services.AddAssemblyOf<AbpAuthorizationModule>();
        }
    }

    public class RequirePermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; set; }
    }

    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(string permissionName)
        {
            Policy = permissionName;
        }
    }

    public class RequirePermissionHandler : AuthorizationHandler<RequirePermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RequirePermissionRequirement requirement)
        {
            if (requirement.PermissionName == "AllowedPermission")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
