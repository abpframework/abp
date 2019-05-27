using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.TestServices;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Authorization
{
    [DependsOn(typeof(AutofacModule))]
    [DependsOn(typeof(AuthorizationModule))]
    public class AbpAuthorizationTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(onServiceRegistredContext =>
            {
                if (typeof(IMyAuthorizedService1).IsAssignableFrom(onServiceRegistredContext.ImplementationType))
                {
                    onServiceRegistredContext.Interceptors.TryAdd<AuthorizationInterceptor>();
                }
            });
        }
    }
}