using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    public class IdentityDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityDataSeeder _identityDataSeeder;

        public IdentityDataSeedContributor(IIdentityDataSeeder identityDataSeeder)
        {
            _identityDataSeeder = identityDataSeeder;
        }

        public Task SeedAsync(DataSeedContext context)
        {
            return _identityDataSeeder.SeedAsync(
                context["AdminEmail"] as string ?? "admin@abp.io",
                context["AdminPassword"] as string ?? "1q2w3E*",
                context.TenantId
            );
        }
    }
}
