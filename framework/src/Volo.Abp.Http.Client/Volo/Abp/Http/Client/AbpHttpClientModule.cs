using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Validation;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Client.ClientProxying.ExtraPropertyDictionaryConverts;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.RemoteServices;

namespace Volo.Abp.Http.Client;

[DependsOn(
    typeof(AbpHttpModule),
    typeof(AbpCastleCoreModule),
    typeof(AbpThreadingModule),
    typeof(AbpMultiTenancyModule),
    typeof(AbpValidationModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpRemoteServicesModule),
    typeof(AbpEventBusModule)
    )]
public class AbpHttpClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient();
        context.Services.AddTransient(typeof(DynamicHttpProxyInterceptorClientProxy<>));

        Configure<AbpHttpClientProxyingOptions>(options =>
        {
            options.QueryStringConverts.Add(typeof(ExtraPropertyDictionary), typeof(ExtraPropertyDictionaryToQueryString));
            options.FormDataConverts.Add(typeof(ExtraPropertyDictionary), typeof(ExtraPropertyDictionaryToFormData));
        });
    }
}
