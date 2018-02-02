using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Ui;

namespace Volo.Abp.MultiTenancy
{
    public class TenantManager : DomainService, ITenantManager
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantManager(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Tenant> CreateAsync(string name)
        {
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name);
            return new Tenant(GuidGenerator.Create(), name);
        }

        public async Task ChangeNameAsync(Tenant tenant, string name)
        {
            Check.NotNull(tenant, nameof(tenant));
            Check.NotNull(name, nameof(name));

            await ValidateNameAsync(name, tenant.Id);
            tenant.SetName(name);
        }

        protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
        {
            var tenant = await _tenantRepository.FindByNameAsync(name);
            if (tenant != null && tenant.Id != expectedId)
            {
                throw new UserFriendlyException("Duplicate tenancy name: " + name); //TODO: A domain exception would be better..?
            }
        }
    }
}