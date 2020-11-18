using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.MultiTenancy
{
    public class WebAssemblyRemoteTenantStore : ITenantStore, ITransientDependency
    {
        protected IHttpClientProxy<IAbpTenantAppService> Proxy { get; }
        protected WebAssemblyRemoteTenantStoreCache Cache { get; }

        public WebAssemblyRemoteTenantStore(
            IHttpClientProxy<IAbpTenantAppService> proxy,
            WebAssemblyRemoteTenantStoreCache cache)
        {
            Proxy = proxy;
            Cache = cache;
        }

        public async Task<TenantConfiguration> FindAsync(string name)
        {
            var cached = Cache.Find(name);
            if (cached != null)
            {
                return cached;
            }

            var tenant = CreateTenantConfiguration(await Proxy.Service.FindTenantByNameAsync(name));
            Cache.Set(tenant);
            return tenant;
        }

        public async Task<TenantConfiguration> FindAsync(Guid id)
        {
            var cached = Cache.Find(id);
            if (cached != null)
            {
                return cached;
            }

            var tenant = CreateTenantConfiguration(await Proxy.Service.FindTenantByIdAsync(id));
            Cache.Set(tenant);
            return tenant;
        }

        public TenantConfiguration Find(string name)
        {
            return AsyncHelper.RunSync(() => FindAsync(name));
        }

        public TenantConfiguration Find(Guid id)
        {
            return AsyncHelper.RunSync(() => FindAsync(id));
        }

        protected virtual TenantConfiguration CreateTenantConfiguration(FindTenantResultDto tenantResultDto)
        {
            if (!tenantResultDto.Success || tenantResultDto.TenantId == null)
            {
                return null;
            }

            return new TenantConfiguration(tenantResultDto.TenantId.Value, tenantResultDto.Name);
        }
    }
}
