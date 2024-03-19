using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.TestServices;
using Volo.Abp.Autofac;
using Volo.Abp.DynamicProxy;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;

namespace Volo.Abp.Authorization;

[DependsOn(typeof(AbpAutofacModule))]
[DependsOn(typeof(AbpAuthorizationModule))]
[DependsOn(typeof(AbpExceptionHandlingModule))]
public class AbpAuthorizationTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(onServiceRegistredContext =>
        {
            if (typeof(IMyAuthorizedService1).IsAssignableFrom(onServiceRegistredContext.ImplementationType) &&
                !DynamicProxyIgnoreTypes.Contains(onServiceRegistredContext.ImplementationType))
            {
                onServiceRegistredContext.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpPermissionOptions>(options =>
        {
            options.ValueProviders.Add<TestPermissionValueProvider1>();
            options.ValueProviders.Add<TestPermissionValueProvider2>();
        });
    }
}
