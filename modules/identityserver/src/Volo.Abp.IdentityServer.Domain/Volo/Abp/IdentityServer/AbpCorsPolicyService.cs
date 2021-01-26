using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityServer.Clients;

namespace Volo.Abp.IdentityServer
{
    public class AbpCorsPolicyService : ICorsPolicyService
    {
        public ILogger<AbpCorsPolicyService> Logger { get; set; }
        protected IHybridServiceScopeFactory HybridServiceScopeFactory { get; }

        public AbpCorsPolicyService(IHybridServiceScopeFactory hybridServiceScopeFactory)
        {
            HybridServiceScopeFactory = hybridServiceScopeFactory;
            Logger = NullLogger<AbpCorsPolicyService>.Instance;
        }

        public virtual async Task<bool> IsOriginAllowedAsync(string origin)
        {
            // doing this here and not in the ctor because: https://github.com/aspnet/AspNetCore/issues/2377
            using (var scope = HybridServiceScopeFactory.CreateScope())
            {
                var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();
                var allowedOrigins = (await clientRepository.GetAllDistinctAllowedCorsOriginsAsync()).ToArray();

                var isAllowed = allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);
                if (!isAllowed)
                {
                    Logger.LogWarning($"Origin is not allowed: {origin}");
                }

                return isAllowed;
            }
        }
    }
}
