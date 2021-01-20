using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.TenantManagement
{
    //TODO: This class should use caching instead of querying everytime!

    public class TenantStore : ITenantStore, ITransientDependency
    {
        protected ITenantRepository TenantRepository { get; }
        protected IObjectMapper<AbpTenantManagementDomainModule> ObjectMapper { get; }
        protected ICurrentTenant CurrentTenant { get; }

        public TenantStore(
            ITenantRepository tenantRepository,
            IObjectMapper<AbpTenantManagementDomainModule> objectMapper,
            ICurrentTenant currentTenant)
        {
            TenantRepository = tenantRepository;
            ObjectMapper = objectMapper;
            CurrentTenant = currentTenant;
        }

        public virtual async Task<TenantConfiguration> FindAsync(string name)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindByNameAsync(name);
                if (tenant == null)
                {
                    return null;
                }

                return ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }

        public virtual async Task<TenantConfiguration> FindAsync(Guid id)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindAsync(id);
                if (tenant == null)
                {
                    return null;
                }

                return ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }

        [Obsolete("Use FindAsync method.")]
        public virtual TenantConfiguration Find(string name)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = TenantRepository.FindByName(name);
                if (tenant == null)
                {
                    return null;
                }

                return ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }

        [Obsolete("Use FindAsync method.")]
        public virtual TenantConfiguration Find(Guid id)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = TenantRepository.FindById(id);
                if (tenant == null)
                {
                    return null;
                }

                return ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
            }
        }
    }
}
