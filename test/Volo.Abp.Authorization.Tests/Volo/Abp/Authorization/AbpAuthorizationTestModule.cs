using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.TestServices;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Authorization
{
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpAuthorizationModule))]
    public class AbpAuthorizationTestModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.OnRegistred(context =>
            {
                if (typeof(IMyAuthorizedService1).IsAssignableFrom(context.ImplementationType))
                {
                    context.Interceptors.TryAdd<AuthorizationInterceptor>();
                }
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.TryAdd<AuthorizationTestPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpAuthorizationTestModule>();
        }
    }
}