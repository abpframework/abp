using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
    {
        protected IHttpClientProxy<IAbpApplicationConfigurationAppService> Proxy { get; }

        protected IMemoryCache Cache { get; }

        public WebAssemblyCachedApplicationConfigurationClient(
            IHttpClientProxy<IAbpApplicationConfigurationAppService> proxy,
            IMemoryCache cache)
        {
            Proxy = proxy;
            Cache = cache;
        }

        public async Task InitializeAsync()
        {
            await GetAsync();
        }

        public async Task<ApplicationConfigurationDto> GetAsync()
        {
            return await Cache.GetOrCreateAsync(
                CreateCacheKey(),
                e => Proxy.Service.GetAsync()
            );
        }

        public ApplicationConfigurationDto Get()
        {
            var cacheKey = CreateCacheKey();
            if(Cache.TryGetValue(cacheKey, out ApplicationConfigurationDto value))
            {
                return value;
            }

            throw new AbpException($"Should initialize the {nameof(ICachedApplicationConfigurationClient)} before getting the value!");
        }

        protected virtual string CreateCacheKey()
        {
            return $"ApplicationConfiguration";
        }
    }
}
