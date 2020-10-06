using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyRemoteTenantStore : ITenantStore, ITransientDependency
    {
        protected IHttpClientProxy<IAbpTenantAppService> Proxy { get; }

        public WebAssemblyRemoteTenantStore(
            IHttpClientProxy<IAbpTenantAppService> proxy)
        {
            Proxy = proxy;
        }

        public async Task<TenantConfiguration> FindAsync(string name)
        {
            //TODO: Cache

            return CreateTenantConfiguration(await Proxy.Service.FindTenantByNameAsync(name));
        }

        public async Task<TenantConfiguration> FindAsync(Guid id)
        {
            //TODO: Cache

            return CreateTenantConfiguration(await Proxy.Service.FindTenantByIdAsync(id));
        }

        public TenantConfiguration Find(string name)
        {
            //TODO: Cache

            return AsyncHelper.RunSync(async () => CreateTenantConfiguration(await Proxy.Service.FindTenantByNameAsync(name)));
        }

        public TenantConfiguration Find(Guid id)
        {
            //TODO: Cache

            return AsyncHelper.RunSync(async () => CreateTenantConfiguration(await Proxy.Service.FindTenantByIdAsync(id)));
        }

        protected virtual TenantConfiguration CreateTenantConfiguration(FindTenantResultDto tenantResultDto)
        {
            if (!tenantResultDto.Success || tenantResultDto.TenantId == null)
            {
                return null;
            }

            return new TenantConfiguration(tenantResultDto.TenantId.Value, tenantResultDto.Name);
        }

        protected virtual string CreateCacheKey(string tenantName)
        {
            return $"RemoteTenantStore_Name_{tenantName}";
        }

        protected virtual string CreateCacheKey(Guid tenantId)
        {
            return $"RemoteTenantStore_Id_{tenantId:N}";
        }
    }
}
