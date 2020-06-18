using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Volo.Abp.Castle;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Validation;

namespace Volo.Abp.Http.Client
{
    [DependsOn(
        typeof(AbpHttpModule),
        typeof(AbpCastleCoreModule),
        typeof(AbpThreadingModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpValidationModule)
        )]
    public class AbpHttpClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpRemoteServiceOptions>(configuration);
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpHttpClientOptions>(options =>
            {
                if (options.HttpClientActions.Any())
                {
                    var httpClientNames = options.HttpClientProxies.Select(x => x.Value.RemoteServiceName);
                    foreach (var httpClientName in httpClientNames)
                    {
                        foreach (var httpClientAction in options.HttpClientActions)
                        {
                            context.Services.Configure<HttpClientFactoryOptions>(httpClientName,
                                x => x.HttpClientActions.Add(httpClientAction.Invoke(httpClientName)));
                        }
                    }
                }
            });
        }
    }
}
