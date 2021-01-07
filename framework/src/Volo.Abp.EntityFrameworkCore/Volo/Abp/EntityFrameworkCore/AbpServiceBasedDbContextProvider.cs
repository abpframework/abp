using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore
{
    public class AbpServiceBasedDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : AbpDbContext<TDbContext>
    {
        private readonly IServiceProvider _serviceProvider;

        public AbpServiceBasedDbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Obsolete("Use GetDbContextAsync method.")]
        public TDbContext GetDbContext()
        {
            return ActivatorUtilities.CreateInstance<TDbContext>(_serviceProvider,
                _serviceProvider.GetRequiredService<IDbContextOptionsProvider<TDbContext>>()
                    .GetDbContextOptions());
        }

        public async Task<TDbContext> GetDbContextAsync()
        {
            return ActivatorUtilities.CreateInstance<TDbContext>(_serviceProvider,
                await _serviceProvider.GetRequiredService<IDbContextOptionsProvider<TDbContext>>()
                    .GetDbContextOptionsAsync());
        }
    }
}
