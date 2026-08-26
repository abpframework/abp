using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity;

public class TestIdentityUserStore : IdentityUserStore
{
    private readonly IdentityUserStoreFailureSimulator _failureSimulator;
    private bool _failNextUpdate;

    public TestIdentityUserStore(
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IGuidGenerator guidGenerator,
        ILogger<IdentityRoleStore> logger,
        ILookupNormalizer lookupNormalizer,
        IdentityErrorDescriber describer,
        IdentityUserStoreFailureSimulator failureSimulator)
        : base(userRepository, roleRepository, guidGenerator, logger, lookupNormalizer, describer)
    {
        _failureSimulator = failureSimulator;
    }

    public override Task ResetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        _failNextUpdate = _failureSimulator.IsAccessFailedCountResetFailureEnabled;
        return base.ResetAccessFailedCountAsync(user, cancellationToken);
    }

    public override Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        if (_failNextUpdate || _failureSimulator.ShouldFailUpdate())
        {
            _failNextUpdate = false;
            return Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "IdentityUserUpdateFailed",
                Description = "The identity user could not be updated."
            }));
        }

        return base.UpdateAsync(user, cancellationToken);
    }
}
