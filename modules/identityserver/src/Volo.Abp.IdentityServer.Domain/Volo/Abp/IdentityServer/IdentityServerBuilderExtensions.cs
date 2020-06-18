using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Devices;
using Volo.Abp.IdentityServer.Grants;

namespace Volo.Abp.IdentityServer
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddAbpStores(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

            return builder
                .AddClientStore<ClientStore>()
                .AddResourceStore<ResourceStore>()
                .AddCorsPolicyService<AbpCorsPolicyService>();
        }
    }
}