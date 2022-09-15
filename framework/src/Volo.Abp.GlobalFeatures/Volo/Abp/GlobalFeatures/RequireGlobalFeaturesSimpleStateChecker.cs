using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.GlobalFeatures;

public class RequireGlobalFeaturesSimpleStateChecker<TState> : ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public string[] GlobalFeatureNames { get; }
    public bool RequiresAll { get; }

    public RequireGlobalFeaturesSimpleStateChecker(params string[] globalFeatureNames)
        : this(true, globalFeatureNames)
    {
    }

    public RequireGlobalFeaturesSimpleStateChecker(bool requiresAll, params string[] globalFeatureNames)
    {
        Check.NotNullOrEmpty(globalFeatureNames, nameof(globalFeatureNames));

        RequiresAll = requiresAll;
        GlobalFeatureNames = globalFeatureNames;
    }

    public RequireGlobalFeaturesSimpleStateChecker(bool requiresAll, params Type[] globalFeatureNames)
    {
        Check.NotNullOrEmpty(globalFeatureNames, nameof(globalFeatureNames));

        RequiresAll = requiresAll;
        GlobalFeatureNames = globalFeatureNames.Select(GlobalFeatureNameAttribute.GetName).ToArray();
    }

    public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context)
    {
        var isEnabled = RequiresAll
            ? GlobalFeatureNames.All(x => GlobalFeatureManager.Instance.IsEnabled(x))
            : GlobalFeatureNames.Any(x => GlobalFeatureManager.Instance.IsEnabled(x));

        return Task.FromResult(isEnabled);
    }
}
