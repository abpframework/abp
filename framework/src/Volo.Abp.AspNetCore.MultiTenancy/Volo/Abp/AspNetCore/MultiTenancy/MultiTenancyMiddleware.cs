using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class MultiTenancyMiddleware : IMiddleware, ITransientDependency
    {
        private readonly ITenantConfigurationProvider _tenantConfigurationProvider;
        private readonly ICurrentTenant _currentTenant;

        public MultiTenancyMiddleware(
            ITenantConfigurationProvider tenantConfigurationProvider,
            ICurrentTenant currentTenant)
        {
            _tenantConfigurationProvider = tenantConfigurationProvider;
            _currentTenant = currentTenant;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tenant = await _tenantConfigurationProvider.GetAsync(saveResolveResult: true);
            using (_currentTenant.Change(tenant?.Id, tenant?.Name))
            {
                await next(context);
            }
        }
    }
}
