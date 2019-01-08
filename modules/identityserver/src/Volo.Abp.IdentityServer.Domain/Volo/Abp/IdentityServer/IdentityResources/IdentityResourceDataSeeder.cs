using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceDataSeeder : IIdentityResourceDataSeeder, ITransientDependency
    {
        protected IIdentityResourceRepository IdentityResourceRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public IdentityResourceDataSeeder(
            IIdentityResourceRepository identityResourceRepository,
            IGuidGenerator guidGenerator)
        {
            IdentityResourceRepository = identityResourceRepository;
            GuidGenerator = guidGenerator;
        }

        public virtual async Task CreateStandardResourcesAsync()
        {
            var resources = new IdentityServer4.Models.IdentityResource[]
            {
                new IdentityServer4.Models.IdentityResources.OpenId(),
                new IdentityServer4.Models.IdentityResources.Profile(),
                new IdentityServer4.Models.IdentityResources.Email(),
                new IdentityServer4.Models.IdentityResources.Address(),
                new IdentityServer4.Models.IdentityResources.Phone()
            };

            foreach (var resource in resources)
            {
                await AddIfNotExistsAsync(resource);
            }
        }

        protected virtual async Task AddIfNotExistsAsync(IdentityServer4.Models.IdentityResource resource)
        {
            if (await IdentityResourceRepository.FindByNameAsync(resource.Name) != null)
            {
                return;
            }

            await IdentityResourceRepository.InsertAsync(
                new IdentityResource(
                    GuidGenerator.Create(),
                    resource
                )
            );
        }
    }
}
