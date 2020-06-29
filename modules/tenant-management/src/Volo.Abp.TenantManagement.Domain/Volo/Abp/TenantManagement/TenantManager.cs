using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.Abp.TenantManagement
{
    public class TenantManager : DomainService, ITenantManager
    {
        protected ITenantRepository TenantRepository { get; }

        public TenantManager(ITenantRepository tenantRepository)
        {
            TenantRepository = tenantRepository;

        }

        public virtual async Task<Tenant> CreateAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name);
            return new Tenant(GuidGenerator.Create(), name);
        }

        public virtual async Task ChangeNameAsync(Tenant tenant, string name)
        {
            Check.NotNull(tenant, nameof(tenant));
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name, tenant.Id);
            tenant.SetName(name);
        }

        protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
        {
            var tenant = await TenantRepository.FindByNameAsync(name);
            if (tenant != null && tenant.Id != expectedId)
            {
                throw new UserFriendlyException("Duplicate tenancy name: " + name); //TODO: A domain exception would be better..?
            }
        }
    }
}