using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.MultiTenancy
{
    public class AbpMultiTenancyTestDataBuilder : ITransientDependency
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantManager _tenantManager;

        public AbpMultiTenancyTestDataBuilder(
            ITenantRepository tenantRepository, 
            ITenantManager tenantManager)
        {
            _tenantRepository = tenantRepository;
            _tenantManager = tenantManager;
        }

        public void Build()
        {
            AsyncHelper.RunSync(AddTenantsAsync);
        }

        private async Task AddTenantsAsync()
        {
            var acme = await _tenantManager.CreateAsync("acme");
            await _tenantRepository.InsertAsync(acme);

            var volosoft = await _tenantManager.CreateAsync("volosoft");
            await _tenantRepository.InsertAsync(volosoft);
        }
    }
}