using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.IdentityServer.IdentityResources;

public class IdentityResourceDataSeeder : IIdentityResourceDataSeeder, ITransientDependency
{
    protected IIdentityClaimTypeRepository ClaimTypeRepository { get; }
    protected IIdentityResourceRepository IdentityResourceRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public IdentityResourceDataSeeder(
        IIdentityResourceRepository identityResourceRepository,
        IGuidGenerator guidGenerator,
        IIdentityClaimTypeRepository claimTypeRepository)
    {
        IdentityResourceRepository = identityResourceRepository;
        GuidGenerator = guidGenerator;
        ClaimTypeRepository = claimTypeRepository;
    }

    public virtual async Task CreateStandardResourcesAsync()
    {
        var resources = new[]
        {
                new IdentityServer4.Models.IdentityResources.OpenId(),
                new IdentityServer4.Models.IdentityResource("Tenant", "Tenant of the user", new[] {"tenantid"})
                {
                   Required = true
                },
                new IdentityServer4.Models.IdentityResources.Profile(),
                new IdentityServer4.Models.IdentityResources.Email(),
                new IdentityServer4.Models.IdentityResources.Address(),
                new IdentityServer4.Models.IdentityResources.Phone(),
                new IdentityServer4.Models.IdentityResource("role", "Roles of the user", new[] {"role"}),
            };

        foreach (var resource in resources)
        {
            foreach (var claimType in resource.UserClaims)
            {
                await AddClaimTypeIfNotExistsAsync(claimType);
            }

            await AddIdentityResourceIfNotExistsAsync(resource);
        }
    }

    protected virtual async Task AddIdentityResourceIfNotExistsAsync(IdentityServer4.Models.IdentityResource resource)
    {
        if (await IdentityResourceRepository.CheckNameExistAsync(resource.Name))
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

    protected virtual async Task AddClaimTypeIfNotExistsAsync(string claimType)
    {
        if (await ClaimTypeRepository.AnyAsync(claimType))
        {
            return;
        }

        await ClaimTypeRepository.InsertAsync(
            new IdentityClaimType(
                GuidGenerator.Create(),
                claimType,
                isStatic: true
            )
        );
    }
}
