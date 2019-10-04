using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace DashboardDemo
{
    public class DashboardDemoTestDataBuilder : ITransientDependency
    {
        private readonly IDataSeeder _dataSeeder;

        public DashboardDemoTestDataBuilder(IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildInternalAsync);
        }

        public async Task BuildInternalAsync()
        {
            await _dataSeeder.SeedAsync();
        }
    }
}