using System;
using Volo.Abp.BackgroundWorkers;

namespace Volo.Abp.OpenIddict.Tokens;

public class TokenCleanupOptions
{
    /// <summary>
    /// Default value: true.
    /// If <see cref="AbpBackgroundWorkerOptions.IsEnabled"/> is false,
    /// this property is ignored and the cleanup worker doesn't work for this application instance.
    /// </summary>
    public bool IsCleanupEnabled { get; set; } = true;

    /// <summary>
    /// Default: 3,600,000 ms.
    /// </summary>
    public int CleanupPeriod { get; set; } = 3_600_000;

    /// <summary>
    /// Gets or sets a boolean indicating whether authorizations pruning should be disabled.
    /// </summary>
    public bool DisableAuthorizationPruning { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating whether tokens pruning should be disabled.
    /// </summary>
    public bool DisableTokenPruning { get; set; }

    /// <summary>
    /// Gets or sets the minimum lifespan authorizations must have to be pruned.
    /// By default, this value is set to 14 days and cannot be less than 10 minutes.
    /// </summary>
    public TimeSpan MinimumAuthorizationLifespan { get; set; } = TimeSpan.FromDays(14);

    /// <summary>
    /// Gets or sets the minimum lifespan tokens must have to be pruned.
    /// By default, this value is set to 14 days and cannot be less than 10 minutes.
    /// </summary>
    public TimeSpan MinimumTokenLifespan { get; set; } = TimeSpan.FromDays(14);
}
