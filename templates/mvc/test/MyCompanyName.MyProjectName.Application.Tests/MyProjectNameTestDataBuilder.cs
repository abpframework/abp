using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameTestDataBuilder : ITransientDependency
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;

        public MyProjectNameTestDataBuilder(IIdentityDataSeeder identityDataSeeder)
        {
            _identityDataSeeder = identityDataSeeder;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildInternalAsync);
        }

        public async Task BuildInternalAsync()
        {
            await _identityDataSeeder.SeedAsync("1q2w3E*");
        }
    }
}