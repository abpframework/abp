using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
    {
        protected IHttpClientProxy<IAbpApplicationConfigurationAppService> Proxy { get; }

        protected ApplicationConfigurationCache Cache { get; }

        public WebAssemblyCachedApplicationConfigurationClient(
            IHttpClientProxy<IAbpApplicationConfigurationAppService> proxy,
            ApplicationConfigurationCache cache)
        {
            Proxy = proxy;
            Cache = cache;
        }

        public virtual async Task InitializeAsync()
        {
            Cache.Set(await Proxy.Service.GetAsync());
        }

        public virtual Task<ApplicationConfigurationDto> GetAsync()
        {
            return Task.FromResult(GetConfigurationByChecking());
        }

        public virtual ApplicationConfigurationDto Get()
        {
            return GetConfigurationByChecking();
        }

        private ApplicationConfigurationDto GetConfigurationByChecking()
        {
            var configuration = Cache.Get();
            if (configuration == null)
            {
                throw new AbpException(
                    $"{nameof(WebAssemblyCachedApplicationConfigurationClient)} should be initialized before using it.");
            }

            return configuration;
        }
    }
}
