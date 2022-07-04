using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.OpenIddict.Tokens;

// Note: this background task is responsible of automatically removing orphaned tokens/authorizations
// (i.e tokens that are no longer valid and ad-hoc authorizations that have no valid tokens associated).
// Import: since tokens associated to ad-hoc authorizations are not removed as part of the same operation,
// the tokens MUST be deleted before removing the ad-hoc authorizations that no longer have any token.
public class TokenCleanupService : ITransientDependency
{
    public ILogger<TokenCleanupService> Logger { get; set; }
    protected TokenCleanupOptions CleanupOptions { get; }
    protected IOpenIddictTokenManager TokenManager { get; }
    protected IOpenIddictAuthorizationManager AuthorizationManager { get; }

    public TokenCleanupService(
        IOptionsMonitor<TokenCleanupOptions> cleanupOptions,
        IOpenIddictTokenManager tokenManager,
        IOpenIddictAuthorizationManager authorizationManager)
    {
        Logger = NullLogger<TokenCleanupService>.Instance;;

        CleanupOptions = cleanupOptions.CurrentValue;
        TokenManager = tokenManager;
        AuthorizationManager = authorizationManager;
    }

    public virtual async Task CleanAsync()
    {
        Logger.LogInformation("Start cleanup.");

        if (!CleanupOptions.DisableTokenPruning)
        {
            Logger.LogInformation("Start cleanup tokens.");

            var threshold = DateTimeOffset.UtcNow - CleanupOptions.MinimumTokenLifespan;
            try
            {
                await TokenManager.PruneAsync(threshold);
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }

        if (!CleanupOptions.DisableAuthorizationPruning)
        {
            Logger.LogInformation("Start cleanup authorizations.");

            var threshold = DateTimeOffset.UtcNow - CleanupOptions.MinimumAuthorizationLifespan;
            try
            {
                await AuthorizationManager.PruneAsync(threshold);
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}
