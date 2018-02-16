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
                    policy.Requirements.Add(new RequiresPermissionRequirement
                    {
                        PermissionName = "AllowedPermission"
                    });
                });

                options.AddPolicy("NotAllowedPermission", policy =>
                {
                    policy.Requirements.Add(new RequiresPermissionRequirement { PermissionName = "NotAllowedPermission" });
                });
            });

            services.AddSingleton<IAuthorizationHandler, RequiresPermissionHandler>();

            services.AddAssemblyOf<AbpAuthorizationModule>();
        }
    }
}
