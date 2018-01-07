using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.MultiTenancy
{
    public class TenantStore : ITenantStore, ITransientDependency
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly ICurrentTenant _currentTenant;

        public TenantStore(
            ITenantRepository tenantRepository, 
            IObjectMapper objectMapper,
            ICurrentTenant currentTenant)
        {
            _tenantRepository = tenantRepository;
            _objectMapper = objectMapper;
            _currentTenant = currentTenant;
        }

        public async Task<TenantInfo> FindAsync(string name)
        {
            using (_currentTenant.Clear()) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await _tenantRepository.FindByNameIncludeDetailsAsync(name);
                if (tenant == null)
                {
                    return null;
                }

                return _objectMapper.Map<Tenant, TenantInfo>(tenant);
            }
        }

        public async Task<TenantInfo> FindAsync(Guid id)
        {
            using (_currentTenant.Clear()) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await _tenantRepository.FindWithIncludeDetailsAsync(id);
                if (tenant == null)
                {
                    return null;
                }

                return _objectMapper.Map<Tenant, TenantInfo>(tenant);
            }
        }
    }
}
