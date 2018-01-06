using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolver : ITenantResolver, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TenantResolveOptions _options;

        public TenantResolver(IOptions<TenantResolveOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public string ResolveTenantIdOrName()
        {
            if (!_options.TenantResolvers.Any())
            {
                return null;
            }

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(serviceScope.ServiceProvider);

                foreach (var tenantResolver in _options.TenantResolvers)
                {
                    tenantResolver.Resolve(context);

                    if (context.HasResolvedTenantOrHost())
                    {
                        return context.TenantIdOrName;
                    }
                }

                //Could not find a tenant
                return null;
            }
        }
    }
}